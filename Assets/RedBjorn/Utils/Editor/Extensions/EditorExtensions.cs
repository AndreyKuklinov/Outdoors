using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.Utils
{
    public static class EditorExtensions
    {
        public static void CenterOnMainWin(this EditorWindow aWin)
        {

            var containerWinType = System.AppDomain.CurrentDomain.GetAllDerivedTypes(typeof(ScriptableObject)).Where(t => t.Name == "ContainerWindow").FirstOrDefault();
            if (containerWinType == null)
            {
                throw new System.MissingMemberException("Can't find internal type ContainerWindow. Maybe something has changed inside Unity");
            }
            var showModeField = containerWinType.GetField("m_ShowMode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var positionProperty = containerWinType.GetProperty("position", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (showModeField == null || positionProperty == null)
            {
                throw new System.MissingFieldException("Can't find internal fields 'm_ShowMode' or 'position'. Maybe something has changed inside Unity");
            }

            Rect main = Rect.zero;
            var windows = Resources.FindObjectsOfTypeAll(containerWinType);
            foreach (var win in windows)
            {
                var showmode = (int)showModeField.GetValue(win);
                if (showmode == 4) // main window
                {
                    main = (Rect)positionProperty.GetValue(win, null);
                    break;
                }
            }
            if (main == Rect.zero)
            {
                throw new System.NotSupportedException("Can't find internal main window. Maybe something has changed inside Unity");
            }
            var pos = aWin.position;
            float w = (main.width - pos.width) * 0.5f;
            float h = (main.height - pos.height) * 0.5f;
            pos.x = main.x + w;
            pos.y = main.y + h;
            aWin.position = pos;
        }
    }
}