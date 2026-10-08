using System;
using SUIF.API;

namespace SUIF.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class UIViewAttribute : Attribute
    {
        public string ViewKey { get; }
        public UILayer Layer { get; }
        public UILoadingMode LoadingMode { get; }
        public bool Preload { get; }
        public bool IsModal { get; }

        public UIViewAttribute(
            string viewKey,
            UILayer layer = UILayer.Default,
            UILoadingMode loadingMode = UILoadingMode.Destroy,
            bool preload = false,
            bool isModal = false)
        {
            ViewKey = viewKey;
            Layer = layer;
            LoadingMode = loadingMode;
            Preload = preload;
            IsModal = isModal;
        }
    }
}
