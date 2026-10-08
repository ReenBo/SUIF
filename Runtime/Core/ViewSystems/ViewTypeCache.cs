using System;
using System.Linq.Expressions;
using System.Reflection;
using SUIF.API;
using SUIF.Base;
using UnityEngine.UIElements;

namespace SUIF.ViewSystems
{
    public static class ViewTypeCache<TView> where TView : class, IView
    {
        public static readonly Type ViewModelType;
        private static readonly Func<VisualElement, TView> ConstructorDelegate;

        static ViewTypeCache()
        {
            var viewType = typeof(TView);
            ViewModelType = GetViewModelType(viewType);
            var constructorInfo = viewType.GetConstructor(new[] { typeof(VisualElement) });

            if (constructorInfo == null)
            {
                throw new InvalidOperationException($"View {viewType.Name} must have a public constructor taking VisualElement.");
            }

            ConstructorDelegate = CreateConstructorDelegate(constructorInfo);
        }

        public static TView CreateInstance(VisualElement root)
        {
            return ConstructorDelegate(root);
        }

        private static Func<VisualElement, TView> CreateConstructorDelegate(ConstructorInfo constructorInfo)
        {
            var parameter = Expression.Parameter(typeof(VisualElement), "root");
            var newExpression = Expression.New(constructorInfo, parameter);
            var lambda = Expression.Lambda<Func<VisualElement, TView>>(newExpression, parameter);
            return lambda.Compile();
        }

        private static Type GetViewModelType(Type viewType)
        {
            var baseType = viewType.BaseType;

            while (baseType is not null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(BaseView<>))
                {
                    return baseType.GetGenericArguments()[0];
                }
                baseType = baseType.BaseType;
            }

            throw new InvalidOperationException($"Failed to find ViewModel type for {viewType.Name}. View must inherit from BaseView<TViewModel>.");
        }
    }
}
