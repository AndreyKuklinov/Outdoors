using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.Utils
{
    public static class InterfaceHelper
    {
        static Dictionary<Type, List<Type>> InterfaceToComponent;
        static List<Type> Types;

        static InterfaceHelper()
        {
            Types = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Types.AddRange(assembly.GetTypes());
            }

            InterfaceToComponent = new Dictionary<Type, List<Type>>();
            foreach (var type in Types)
            {
                if (!type.IsInterface)
                {
                    continue;
                }

                var typeName = type.ToString().ToLower();
                if (typeName.Contains("unity")
                    || typeName.Contains("system.")
                    || typeName.Contains("mono.")
                    || typeName.Contains("icsharpcode.")
                    || typeName.Contains("nsubstitute")
                    || typeName.Contains("nunit.")
                    || typeName.Contains("microsoft.")
                    || typeName.Contains("boo.")
                    || typeName.Contains("serializ")
                    || typeName.Contains("json")
                    || typeName.Contains("log.")
                    || typeName.Contains("logging")
                    || typeName.Contains("test")
                    || typeName.Contains("editor")
                    || typeName.Contains("debug"))
                {
                    continue;
                }

                var typesInherited = new List<Type>();
                foreach (var cached in Types)
                {
                    if (type.IsAssignableFrom(cached) && cached.IsSubclassOf(typeof(Component)))
                    {
                        typesInherited.Add(cached);
                    }
                }

                if (typesInherited.Count > 0)
                {
                    var components = new List<Type>();
                    foreach (var typeInherited in typesInherited)
                    {
                        var t = typeof(Component);
                        if (!typeInherited.IsInterface
                            && t == typeInherited || typeInherited.IsSubclassOf(t))
                        {
                            if (!components.Contains(typeInherited))
                            {
                                components.Add(typeInherited);
                            }
                        }
                    }
                    InterfaceToComponent.Add(type, components);
                }
            }
        }

        public static IList<T> FindObjects<T>() where T : class
        {
            var tType = typeof(T);
            var types = InterfaceToComponent.TryGetOrDefault(tType);
            if (types == null || types.Count <= 0)
            {
                Log.E($"No descendants found for type {tType}");
                return null;
            }

            var result = new List<T>();
            foreach (var type in types)
            {
#if UNITY_2023_1_OR_NEWER
                var objects = UnityEngine.Object.FindObjectsByType(type, FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
#else
                var objects = UnityEngine.Object.FindObjectsOfType(type);
#endif
                if (objects == null || objects.Length == 0)
                {
                    continue;
                }

                foreach (var obj in objects)
                {
                    if (obj is T casted)
                    {
                        result.Add(casted);
                    }
                }
            }
            return result;
        }
    }
}