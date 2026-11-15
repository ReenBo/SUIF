using System;
using SUIF.Binding;
using UnityEngine.UIElements;

namespace SUIF.Controls
{
    [UxmlElement("FixedSlotLabel")]
    public partial class FixedSlotLabel : Label
    {
        private const int DefaultCapacity = 32;

        private int _capacity = DefaultCapacity;
        private FixedTextSlot _slot;

        [UxmlAttribute("capacity")]
        public int Capacity
        {
            get => _capacity;
            set
            {
                int normalized = value < 1 ? 1 : value;
                if (_capacity == normalized)
                {
                    return;
                }

                _capacity = normalized;
                _slot?.SetCapacity(normalized);
            }
        }

        public FixedSlotLabel()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
        }

        public void SetFixedText(string value)
        {
            EnsureSlot().SetText(value);
        }

        public void SetFixedText(ReadOnlySpan<char> value)
        {
            EnsureSlot().SetText(value);
        }

        public void SetFixedFormat<T1>(string format, T1 arg1)
        {
            EnsureSlot().SetFormat(format, arg1);
        }

        public void SetFixedFormat<T1, T2>(string format, T1 arg1, T2 arg2)
        {
            EnsureSlot().SetFormat(format, arg1, arg2);
        }

        public void SetFixedWritten<TState>(TState state, FixedTextWriter<TState> writer)
        {
            EnsureSlot().SetWritten(state, writer);
        }

        private FixedTextSlot EnsureSlot()
        {
            return _slot ??= new FixedTextSlot(this, _capacity);
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            EnsureSlot();
        }
    }
}
