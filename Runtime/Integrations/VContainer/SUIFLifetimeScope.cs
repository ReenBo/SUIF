using SUIF.API;
using SUIF.ViewSystems;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SUIF.VContainer
{
    public abstract class SUIFLifetimeScope : LifetimeScope
    {
        [SerializeField] private UIRoot _uiRootPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            if (_uiRootPrefab != null)
            {
                builder.RegisterComponentInNewPrefab(_uiRootPrefab, Lifetime.Singleton).As<IUIRoot>();
            }

            builder.RegisterSUIF();
            ConfigureWindows(builder);
        }

        protected virtual void ConfigureWindows(IContainerBuilder builder)
        {
            // Override to register project ViewModels and Services
        }
    }
}
