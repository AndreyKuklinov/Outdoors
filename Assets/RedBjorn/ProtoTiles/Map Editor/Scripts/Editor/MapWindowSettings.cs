using RedBjorn.Utils;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    public class MapWindowSettings : ScriptableObjectExtended
    {
        [Serializable]
        public class Theme
        {
            public Color CommonColor;
            public Color SeparatorColor;
        }

        public Theme Light;
        public Theme Dark;

        public MapRules Rules;

        public float Border;
        public float FoldoutHeight;
        public float CreatorsHeight;
        public float RulesHeight;
        public float PainterHeight;
        public float ToolLabelWidth;
        public float TileLabelWidth;

        public Color LabelColor;
        public Color EdgeColor;
        public Color EdgeCursorColor;
        public Color EdgeCursorPaint;
        public Color EdgeCursorErase;
        public Color TileCursorBrush;
        public Color TileCursorErase;

        public Texture2D BrushIcon;
        public Texture2D EraseIcon;
        public GUISkin Skin;

        [HideInInspector] public bool DrawSideTool;
        public bool AreasAutoMark;
        public bool ShowWalkable;
        public bool ShowGrid;
        public Material CellBorder;
        public Shader ShaderDraw2D;
        public Shader ShaderDraw3D;

        public const string DefaultPathFull = Utils.Paths.ScriptablePath.RootFolder + "/" + DefaultPathRelative;
        public const string DefaultPathRelative = "RedBjorn/ProtoTiles/Map Editor/Editor Resources/MapWindowSettings.asset";
        public static Vector2 WindowMinSize = new Vector2(270f, 480f);

        public Color ColorCreators => EditorGUIUtility.isProSkin ? Dark.CommonColor : Light.CommonColor;
        public Color ColorRules => EditorGUIUtility.isProSkin ? Dark.CommonColor : Light.CommonColor;
        public Color ColorPainter => EditorGUIUtility.isProSkin ? Dark.CommonColor : Light.CommonColor;
        public Color ColorPresets => EditorGUIUtility.isProSkin ? Dark.CommonColor : Light.CommonColor;
        public Color ColorSeparator => EditorGUIUtility.isProSkin ? Dark.SeparatorColor : Light.SeparatorColor;

        public static MapWindowSettings Instance
        {
            get
            {
                var path = DefaultPathFull;
                var instance = AssetDatabase.LoadAssetAtPath<MapWindowSettings>(DefaultPathFull);
                if (!instance)
                {
                    var paths = AssetDatabase.FindAssets("t:" + nameof(MapWindowSettings))
                         .Select(a => AssetDatabase.GUIDToAssetPath(a))
                         .OrderBy(a => a);
                    path = paths.FirstOrDefault(i => i.Contains(DefaultPathRelative));
                    instance = AssetDatabase.LoadAssetAtPath<MapWindowSettings>(path);
                    if (!instance)
                    {
                        path = paths.FirstOrDefault();
                        instance = AssetDatabase.LoadAssetAtPath<MapWindowSettings>(path);
                    }
                }
                return instance;
            }
        }
    }
}
