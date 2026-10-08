using System;
using System.Collections.Generic;
using System.Reflection;
using SUIF.Attributes;
using UnityEngine.UIElements;

namespace SUIF.Binding
{
    public static class UQueryResolver
    {
        private struct BindingInfo
        {
            public FieldInfo Field;
            public string Query;
        }

        private static readonly Dictionary<Type, BindingInfo[]> Cache = new();

        public static void Resolve(object target, VisualElement root)
        {
            var type = target.GetType();

            if (!Cache.TryGetValue(type, out var bindings))
            {
                bindings = CreateBindings(type);
                Cache[type] = bindings;
            }

            for (var i = 0; i < bindings.Length; i++)
            {
                var binding = bindings[i];
                var element = root.Q(binding.Query);

                if (element != null)
                {
                    binding.Field.SetValue(target, element);
                }
                else
                {
                    UnityEngine.Debug.LogError($"[SUIF.UQueryResolver] Element '{binding.Query}' not found in {type.Name}");
                }
            }
        }

        private static BindingInfo[] CreateBindings(Type type)
        {
            var list = new List<BindingInfo>();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            for (var index = 0; index < fields.Length; index++)
            {
                var field = fields[index];
                var attributes = field.GetCustomAttributes(typeof(UQueryAttribute), true);

                if (attributes.Length > 0)
                {
                    list.Add(new BindingInfo
                    {
                        Field = field,
                        Query = ((UQueryAttribute)attributes[0]).Query
                    });
                }
            }

            return list.ToArray();
        }

        public static void ClearCache()
        {
            Cache.Clear();
        }
    }
}
