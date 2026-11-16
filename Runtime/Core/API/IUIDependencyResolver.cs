using System;

namespace SUIF.API
{
    public interface IUIDependencyResolver
    {
        T Resolve<T>();
        object Resolve(Type type);
        bool TryResolve<T>(out T resolved);
    }
}
