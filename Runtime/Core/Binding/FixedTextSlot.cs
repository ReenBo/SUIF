using System;
using Cysharp.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace SUIF.Binding
{
    public delegate void FixedTextWriter<in TState>(ref Utf16ValueStringBuilder builder, TState state);

    public sealed class FixedTextSlot : IDisposable
    {
        private const string HiddenSizePrefix = "<size=0.01>";
        private const string HiddenSizeSuffix = "</size>";
        private const char PaddingChar = '0';
        private const int WrapperOverhead = 18;

        private readonly TextElement _target;
        private readonly Action<TextElement.GlyphsEnumerable> _collapseCallback;
        private readonly bool _clampToCapacity;

        private int _capacity;
        private char[] _buffer;
        private int _lastRawVisibleLength = -1;
        private int _lastGlyphCutoffIndex;
        private bool _disposed;

        public int Capacity => _capacity;
        public TextElement Target => _target;

        public FixedTextSlot(TextElement target, int capacity = 32, string initialText = null, bool clampToCapacity = true)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            if (capacity < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be at least 1.");
            }

            _capacity = capacity;
            _clampToCapacity = clampToCapacity;
            _collapseCallback = CollapsePaddingGlyphs;

            EnsureBufferCapacity(capacity + WrapperOverhead + 64);

            _target.enableRichText = true;
            _target.PostProcessTextVertices += _collapseCallback;

            if (initialText != null)
            {
                SetText(initialText);
            }
            else if (!string.IsNullOrEmpty(_target.text))
            {
                SetText(_target.text);
            }
            else
            {
                SetText(ReadOnlySpan<char>.Empty);
            }
        }

        public void SetText(string value)
        {
            if (value == null)
            {
                SetText(ReadOnlySpan<char>.Empty);
                return;
            }

            SetText(value.AsSpan());
        }

        public void SetText(ReadOnlySpan<char> value)
        {
            if (_disposed)
            {
                return;
            }

            MeasureVisibleSpan(value, out int acceptedRawLength, out int parsedVisibleLength, out int emittedQuadCount);

            if (acceptedRawLength == _lastRawVisibleLength &&
                value.Slice(0, acceptedRawLength).SequenceEqual(_buffer.AsSpan(0, acceptedRawLength)))
            {
                return;
            }

            int paddingCount = _capacity - emittedQuadCount;
            if (paddingCount < 0)
            {
                paddingCount = 0;
            }

            int requiredTotal = acceptedRawLength + (paddingCount > 0 ? WrapperOverhead + paddingCount : 0);
            EnsureBufferCapacity(requiredTotal);

            value.Slice(0, acceptedRawLength).CopyTo(_buffer.AsSpan());
            int writeIndex = acceptedRawLength;

            if (paddingCount > 0)
            {
                HiddenSizePrefix.AsSpan().CopyTo(_buffer.AsSpan(writeIndex));
                writeIndex += HiddenSizePrefix.Length;

                _buffer.AsSpan(writeIndex, paddingCount).Fill(PaddingChar);
                writeIndex += paddingCount;

                HiddenSizeSuffix.AsSpan().CopyTo(_buffer.AsSpan(writeIndex));
                writeIndex += HiddenSizeSuffix.Length;
            }

            _lastRawVisibleLength = acceptedRawLength;
            _lastGlyphCutoffIndex = parsedVisibleLength;

            _target.SetText(_buffer, 0, writeIndex);
        }

        public void SetFormat<T1>(string format, T1 arg1)
        {
            if (_disposed)
            {
                return;
            }

            using var builder = ZString.CreateStringBuilder(true);
            builder.AppendFormat(format, arg1);
            SetText(builder.AsSpan());
        }

        public void SetFormat<T1, T2>(string format, T1 arg1, T2 arg2)
        {
            if (_disposed)
            {
                return;
            }

            using var builder = ZString.CreateStringBuilder(true);
            builder.AppendFormat(format, arg1, arg2);
            SetText(builder.AsSpan());
        }

        public void SetWritten<TState>(TState state, FixedTextWriter<TState> writer)
        {
            if (_disposed || writer == null)
            {
                return;
            }

            var builder = ZString.CreateStringBuilder(true);
            try
            {
                writer(ref builder, state);
                SetText(builder.AsSpan());
            }
            finally
            {
                builder.Dispose();
            }
        }

        public void SetCapacity(int newCapacity)
        {
            if (newCapacity < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(newCapacity), "Capacity must be at least 1.");
            }

            if (_capacity == newCapacity)
            {
                return;
            }

            _capacity = newCapacity;
            int prevLen = _lastRawVisibleLength >= 0 ? _lastRawVisibleLength : 0;
            _lastRawVisibleLength = -1;
            var snapshot = prevLen > 0 ? new string(_buffer, 0, prevLen) : string.Empty;
            SetText(snapshot);
        }

        private void EnsureBufferCapacity(int requiredLength)
        {
            if (_buffer != null && _buffer.Length >= requiredLength)
            {
                return;
            }

            int nextSize = _buffer == null ? requiredLength : Math.Max(requiredLength, _buffer.Length * 2);
            _buffer = new char[nextSize];
        }

        private void MeasureVisibleSpan(ReadOnlySpan<char> value, out int acceptedRawLength, out int parsedVisibleLength, out int emittedQuadCount)
        {
            acceptedRawLength = 0;
            parsedVisibleLength = 0;
            emittedQuadCount = 0;
            bool insideTag = false;

            for (int i = 0; i < value.Length; i++)
            {
                char ch = value[i];
                if (ch == '<')
                {
                    insideTag = true;
                    acceptedRawLength = i + 1;
                    continue;
                }

                if (insideTag)
                {
                    if (ch == '>')
                    {
                        insideTag = false;
                    }

                    acceptedRawLength = i + 1;
                    continue;
                }

                if (ch == '\r' || ch == '\n')
                {
                    parsedVisibleLength++;
                    acceptedRawLength = i + 1;
                    continue;
                }

                if (_clampToCapacity && emittedQuadCount >= _capacity)
                {
                    break;
                }

                emittedQuadCount++;
                parsedVisibleLength++;
                acceptedRawLength = i + 1;
            }
        }

        private void CollapsePaddingGlyphs(TextElement.GlyphsEnumerable glyphs)
        {
            int cutoff = _lastGlyphCutoffIndex;
            foreach (var glyph in glyphs)
            {
                if (glyph.textRange.start < cutoff)
                {
                    continue;
                }

                var vertices = glyph.vertices;
                for (int i = 0; i < vertices.Length; i++)
                {
                    var vertex = vertices[i];
                    vertex.position = Vector3.zero;
                    vertex.tint = default;
                    vertices[i] = vertex;
                }
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _target.PostProcessTextVertices -= _collapseCallback;
        }
    }
}
