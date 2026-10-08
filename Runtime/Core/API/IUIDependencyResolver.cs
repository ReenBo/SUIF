using System;

namespace SUIF.API
{
    /// <summary>
    /// Abstract dependency injection adapter for resolving ViewModels and services.
    /// Implemented out-of-the-box by SUIF.VContainer or custom DI containers.
    /// </summary>
    public interface IUIDependencyResolver
    {
        T Resolve<T>();
        object Resolve(Type type);
        bool TryResolve<T>(out T resolved);
    }
}
