using SUIF.API;
using SUIF.ViewSystems;
using VContainer;
using VContainer.Unity;

namespace SUIF.VContainer
{
    public static class SUIFVContainerExtensions
    {
        public static void RegisterSUIF(this IContainerBuilder builder)
        {
            builder.Register<VContainerUIDependencyResolver>(Lifetime.Singleton).As<IUIDependencyResolver>();
            builder.Register<ViewCache>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<ViewFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<UIWindowManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<UIThemeService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<UIFlow>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
