using System;
using System.Collections;
using System.Reflection;
using UnityEditor;

namespace RedBjorn.Utils
{
    public static class SerializedPropertyExtensions
    {
        const char Delimeter = '.';
        const string IndexStartSymbols = "[";
        const string IndexFinishSymbols = "]";
        const string ArrayStartSymbols = ".Array.data[";

        public static object GetValue(this SerializedProperty property)
        {
            object source = null;
            if (property != null)
            {
                source = property.serializedObject.targetObject;
                var path = property.propertyPath.Replace(ArrayStartSymbols, IndexStartSymbols);
                var pathSplitted = path.Split(Delimeter);
                foreach (var pathSplit in pathSplitted)
                {
                    if (pathSplit.Contains(IndexStartSymbols))
                    {
                        var indexStart = pathSplit.IndexOf(IndexStartSymbols);
                        var memberName = pathSplit.Substring(0, indexStart);
                        if (GetMemberFieldPropertyValue(source, memberName) is IEnumerable enumerable)
                        {
                            var enumerator = enumerable.GetEnumerator();
                            var pathCleared = pathSplit.Substring(indexStart).Replace(IndexStartSymbols, string.Empty)
                                                                             .Replace(IndexFinishSymbols, string.Empty);
                            try
                            {
                                var index = Convert.ToInt32(pathCleared);
                                for (var i = 0; i <= index; i++)
                                {
                                    if (!enumerator.MoveNext())
                                    {
                                        source = null;
                                        break;
                                    }
                                }
                                source = enumerator.Current;
                            }
                            catch
                            {
                                source = null;
                            }
                        }
                    }
                    else
                    {
                        source = GetMemberFieldPropertyValue(source, pathSplit);
                    }
                }
            }
            return source;
        }

        static object GetMemberFieldPropertyValue(object source, string memberName)
        {
            if (source != null)
            {
                var type = source.GetType();
                while (type != null)
                {
                    var field = type.GetField(memberName, BindingFlags.NonPublic
                                                            | BindingFlags.Public
                                                            | BindingFlags.Instance);
                    if (field != null)
                    {
                        return field.GetValue(source);
                    }

                    var property = type.GetProperty(memberName, BindingFlags.NonPublic
                                                                | BindingFlags.Public
                                                                | BindingFlags.Instance
                                                                | BindingFlags.IgnoreCase);
                    if (property != null)
                    {
                        return property.GetValue(source, null);
                    }
                    type = type.BaseType;
                }
            }
            return null;
        }
    }
}
