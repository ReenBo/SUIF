using System;
using SUIF.API;
using SUIF.Attributes;

namespace SUIF.ViewSystems
{
    public struct ViewData : IEquatable<ViewData>
    {
        public Type ViewType;
        public IView View;
        public UILoadingMode LoadingMode;
        public UILayer Layer;
        public bool IsModal;
        public string ViewKey;

        public bool Equals(ViewData other)
        {
            return ViewType == other.ViewType && ReferenceEquals(View, other.View);
        }

        public override bool Equals(object obj)
        {
            return obj is ViewData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ViewType, View);
        }

        public static bool operator ==(ViewData left, ViewData right) => left.Equals(right);
        public static bool operator !=(ViewData left, ViewData right) => !left.Equals(right);
    }
}
