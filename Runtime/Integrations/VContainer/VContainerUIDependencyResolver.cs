using System;
using SUIF.API;
using VContainer;

namespace SUIF.VContainer
{
    public class VContainerUIDependencyResolver : IUIDependencyResolver
    {
        private readonly IObjectResolver _resolver;

        public VContainerUIDependencyResolver(IObjectResolver resolver)
        {
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        public T Resolve<T>() => _resolver.Resolve<T>();

        public object Resolve(Type type) => _resolver.Resolve(type);

        public bool TryResolve<T>(out T resolved)
        {
            try
            {
                resolved = _resolver.Resolve<T>();
                return true;
            }
            catch
            {
                resolved = default;
                return false;
            }
        }
    }
}
