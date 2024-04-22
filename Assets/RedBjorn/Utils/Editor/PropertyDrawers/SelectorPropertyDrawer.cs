using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.Utils
{
    [CustomPropertyDrawer(typeof(SelectorAttribute), true)]
    public class SelectorDrawer : PropertyDrawer
    {
        const string NullLabel = "None";

        static readonly Dictionary<string, Type[]> Cache = new Dictionary<string, Type[]>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var typename = property.managedReferenceFieldTypename;
            var splitted = typename.Split(' ');
            typename = splitted.Length > 1 ? $"{splitted[1]},{splitted[0]}" : typename;

            var types = Cache.TryGetOrDefault(typename);
            if (types == null)
            {
                var baseType = Type.GetType(typename);
                if (baseType != null)
                {
                    types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                                                                   .Where(p => !p.IsAbstract
                                                                                && !p.IsInterface
                                                                                && !p.ContainsGenericParameters
                                                                                && baseType.IsAssignableFrom(p))
                                                                   .ToArray();
                    Cache.Add(typename, types);
                }
            }

            string[] typenames = null;
            if (types != null)
            {
                typenames = new string[types.Length + 1];
                typenames[0] = NullLabel;
                for (int i = 0; i < types.Length; i++)
                {
                    typenames[i + 1] = GetCaptionType(types[i]);
                }
            }
            else
            {
                typenames = new string[] { NullLabel };
            }
            var target = property.GetValue();
            var indexCurrent = target != null ? Array.IndexOf(typenames, GetCaptionType(target.GetType())) : -1;
            indexCurrent = Mathf.Max(indexCurrent, 0);
            var rectSize = position.size;
            position.height = EditorGUIUtility.singleLineHeight;
            var indexNew = EditorGUI.Popup(position, label.text, indexCurrent, typenames);

            if (indexCurrent >= 0 && indexNew != indexCurrent)
            {
                if (indexNew <= 0)
                {
                    property.managedReferenceValue = null;
                }
                else
                {
                    property.managedReferenceValue = Activator.CreateInstance(types[indexNew - 1]);
                }
                property.serializedObject.ApplyModifiedProperties();
            }

            position.size = rectSize;
#if UNITY_2022_2_OR_NEWER
            if (property.serializedObject.targetObject is MonoBehaviour)
            {
                var shift = 12f;
                EditorGUIUtility.labelWidth += shift;
                position.x -= shift;
            }
#endif
            EditorGUI.PropertyField(position, property, GUIContent.none, includeChildren: true);
        }

        string GetCaptionType(Type type)
        {
            if (type == null)
            {
                return NullLabel;
            }
            var caption = type.GetCustomAttributes(typeof(CaptionAttribute), true).FirstOrDefault() as CaptionAttribute;
            return caption != null && !string.IsNullOrEmpty(caption.Text)
                                    ? caption.Text
                                    : type.Name;
        }
    }
}
