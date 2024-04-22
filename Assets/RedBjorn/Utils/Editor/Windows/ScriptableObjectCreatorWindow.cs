using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.Utils
{
    public class ScriptableObjectCreatorWindow : EditorWindowExtended
    {
        public Vector2 MenuPos;
        public Vector2 CommonPos;
        public Rect Menu;
        public Rect MenuContent;
        public Rect Common;
        public Rect CommonContent;
        public Rect Create;
        public Type[] Types;
        public string[] TypeNames;
        public int TypeSelected;
        public GUIStyle Style;
        public Color BackgroundColor;
        public ScriptableObject Current;
        public Editor CurrentEditor;

        [MenuItem("Window/ScriptableObject Creator")]
        public static void DoShow()
        {
            var window = (ScriptableObjectCreatorWindow)EditorWindow.GetWindow(typeof(ScriptableObjectCreatorWindow));
            window.titleContent = new GUIContent("ScriptableObject Creator");
            window.Style = new GUIStyle(EditorStyles.miniButton);
            window.Style.alignment = TextAnchor.MiddleLeft;
            window.BackgroundColor = EditorGUIUtility.isProSkin ? new Color(0.1f, 0.1f, 0.1f, 1f) : new Color(0.745f, 0.745f, 0.745f, 1f);
            window.Show();
        }

        void OnGUI()
        {
            var scale = EditorGUIUtility.pixelsPerPoint;
            var windowWidth = Screen.width / scale;
            var windowHeight = Screen.height / scale;
            var border = 4f;

            Menu.x = 2 * border;
            Menu.y = 2 * border;
            Menu.width = windowWidth / 3f - 3 * border;
            Menu.height = windowHeight - 100;
            EditorGUI.DrawRect(Menu, BackgroundColor);
            MenuContent.x = Menu.x + 2 * border;
            MenuContent.y = Menu.y + 2 * border;
            MenuContent.width = Menu.width - 4 * border;
            MenuContent.height = Menu.height - 4 * border;

            GUILayout.BeginArea(MenuContent);
            MenuPos = GUILayout.BeginScrollView(MenuPos);
            var current = TypeSelected;
            TypeSelected = GUILayout.SelectionGrid(TypeSelected, TypeNames, 1, Style);
            if (!Current || current != TypeSelected)
            {
                Current = ScriptableObject.CreateInstance(TypeNames[TypeSelected]);
                CurrentEditor = Editor.CreateEditor(Current);
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();

            Common.x = Menu.x + Menu.width + 2 * border;
            Common.y = 2 * border;
            Common.width = windowWidth / 3 * 2 - 3 * border;
            Common.height = windowHeight - 100;
            EditorGUI.DrawRect(Common, BackgroundColor);
            CommonContent.x = Common.x + 2 * border;
            CommonContent.y = Common.y + 2 * border;
            CommonContent.width = Common.width - 4 * border;
            CommonContent.height = Common.height - 4 * border;

            GUILayout.BeginArea(CommonContent);
            CommonPos = GUILayout.BeginScrollView(CommonPos);
            if (CurrentEditor)
            {
                CurrentEditor.DrawDefaultInspector();
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();

            Create.x = 0.7f * windowWidth;
            Create.y = Common.y + Common.height + 15f;
            Create.width = 0.3f * windowWidth - 3 * border;
            Create.height = 85f;
            GUILayout.BeginArea(Create);
            if (GUILayout.Button("Create"))
            {
                if (Current)
                {
                    var newObj = Instantiate(Current);
                    var assetName = TypeNames[TypeSelected].Split('.').Last();
                    var path = $"{assetName}.asset";
                    var folder = "Assets";
                    var guid = Selection.assetGUIDs.FirstOrDefault();
                    if (!string.IsNullOrEmpty(guid))
                    {
                        var testPath = AssetDatabase.GUIDToAssetPath(guid);
                        if (!Directory.Exists(testPath))
                        {
                            testPath = Path.GetDirectoryName(testPath);

                        }
                        if (Directory.Exists(testPath))
                        {
                            folder = testPath;
                        }
                    }
                    path = Path.Combine(folder, path);
                    path = AssetDatabase.GenerateUniqueAssetPath(path);
                    AssetDatabase.CreateAsset(newObj, path);
                    Selection.activeObject = newObj;
                }
                else
                {
                    Log.E("There is no valid scriptable object");
                }
            }
            GUILayout.EndArea();
        }

        void OnEnable()
        {

            Types = System.AppDomain.CurrentDomain.GetAllDerivedTypes(typeof(ScriptableObjectExtended))
                                                  .Where(t => !t.IsAbstract)
                                                  .OrderBy(t => t.FullName)
                                                  .ToArray();
            TypeNames = Types.Select(s => s.FullName).ToArray();
        }
    }
}