using System;
using SUIF.API;

namespace SUIF.Common
{
    public class DefaultDependencyResolver : IUIDependencyResolver
    {
        public T Resolve<T>() => (T)Resolve(typeof(T));

        public object Resolve(Type type)
        {
            return Activator.CreateInstance(type);
        }

        public bool TryResolve<T>(out T resolved)
        {
            try
            {
                resolved = Resolve<T>();
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
