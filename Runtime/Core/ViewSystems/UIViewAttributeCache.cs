using System;
using System.Reflection;
using SUIF.Attributes;

namespace SUIF.ViewSystems
{
    public static class UIViewAttributeCache<TView>
    {
        public static readonly UIViewAttribute Attribute;

        static UIViewAttributeCache()
        {
            Attribute = typeof(TView).GetCustomAttribute<UIViewAttribute>(false);
        }
    }
}
