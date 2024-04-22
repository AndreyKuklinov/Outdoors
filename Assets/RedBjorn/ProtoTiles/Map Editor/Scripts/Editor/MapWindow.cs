using RedBjorn.Utils;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    public class MapWindow : EditorWindowExtended
    {
        public bool ShowWalkable;
        public bool ShowGrid;
        public float WindowWidth;
        public float WindowHeight;

        [SerializeField]
        int GridAxisType;
        [SerializeField]
        int GridType;
        [SerializeField]
        int RotationType;
        [SerializeField]
        int TilePresetCurrent;
        [SerializeField]
        int TilePresetPrefab;
        [SerializeField]
        int ToolCurrent;
        [SerializeField]
        bool RulesFoldout;
        [SerializeField]
        bool PainterFoldout;
        [SerializeField]
        bool PresetsFoldout;

        Rect Creator;
        Rect Rules;
        Rect Painter;
        Rect Presets;
        Vector2 ScrollPos;
        bool GuiEnabled;
        bool[] ToolToogle = new bool[4];
        bool[] ToolTooglePrevious = new bool[4];

        static readonly string[] TileToolNames = new string[]
        {
            "Brush",
            "Eraser"
        };

        static readonly string[] EdgeToolNames = new string[]
        {
            "Brush",
            "Eraser"
        };

        static readonly string[] Grids = System.Enum.GetNames(typeof(GridType));
        static readonly string[] GridAxes = System.Enum.GetNames(typeof(GridAxis));
        static readonly string[] Rotations = System.Enum.GetNames(typeof(RotationType));

        public MapWindowSettings Settings { get; private set; }

        [SerializeField]
        MapSceneDrawer CachedSceneDrawer;
        public MapSceneDrawer SceneDrawer
        {
            get
            {
                if (CachedSceneDrawer == null)
                {
                    CachedSceneDrawer = new MapSceneDrawer() { Window = this, WindowSettings = Settings };
                }
                return CachedSceneDrawer;
            }
        }

        bool Editable { get { return Map != null; } }
        bool IsSceneEditing { get { return Editable && ToolToogle.Any(t => t); } }
        bool IsSceneEditingPrevious { get { return Editable && ToolTooglePrevious.Any(t => t); } }

        float FoldoutHeight => Settings.FoldoutHeight;
        public float Border => Settings.Border;
        float ToolLabelWidth => Settings.ToolLabelWidth;
        float TileLabelWidth => Settings.TileLabelWidth;
        Color Separator => Settings.ColorSeparator;
        GUISkin Skin => Settings.Skin;

        [SerializeField]
        MapSettings CachedMap;
        MapSettings Map
        {
            get
            {
                return CachedMap;
            }
            set
            {
                if (CachedMap != value)
                {
                    CachedMap = value;
                    OnChangedMap();
                }
            }
        }

        [MenuItem("Tools/Red Bjorn/Editors/Map")]
        public static void DoShow()
        {
            DoShow(null);
        }

        public static void DoShow(MapSettings map)
        {
            var window = (MapWindow)EditorWindow.GetWindow(typeof(MapWindow));
            window.minSize = MapWindowSettings.WindowMinSize;
            window.titleContent = new GUIContent("Map Editor");
            window.Init();
            window.Map = map;
            window.Show();
        }

        void OnEnable()
        {
            InitResources();
            Undo.undoRedoPerformed += OnUndoRedoPerformed;
        }

        void OnDisable()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
            SceneDrawer.Release();
            Undo.undoRedoPerformed -= OnUndoRedoPerformed;
        }

        void OnGUI()
        {
            var scale = EditorGUIUtility.pixelsPerPoint;
            WindowWidth = Screen.width / scale;
            WindowHeight = Screen.height / scale;
            GuiEnabled = GUI.enabled;

            var x = 2 * Border;
            var width = WindowWidth - 4 * Border;
            RectDraw(ref Creator, x, 2 * Border, width, Settings.CreatorsHeight, Settings.ColorCreators);
            AreaCreator();
            RectDraw(ref Rules, x, Creator.y + Creator.height + 2 * Border, width, RulesFoldout ? Settings.RulesHeight : FoldoutHeight, Settings.ColorRules);
            AreaRules();
            RectDraw(ref Painter, x, Rules.y + Rules.height + 2 * Border, width, PainterFoldout ? Settings.PainterHeight : FoldoutHeight, Settings.ColorPainter);
            AreaPainter();
            var heightRest = WindowHeight - Creator.height - Rules.height - Painter.height - 4 * 2 * 2 * Border;
            RectDraw(ref Presets, x, Painter.y + Painter.height + 2 * Border, width, PresetsFoldout ? heightRest : FoldoutHeight, Settings.ColorPresets);
            AreaPresets();
        }

        void OnFocus()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
            SceneView.duringSceneGui += this.OnSceneGUI;
            if (SceneDrawer != null)
            {
                SceneDrawer.OnBeforeChanged += UndoRecord;
            }
        }

        void OnLostFocus()
        {
            if (SceneDrawer != null)
            {
                SceneDrawer.OnBeforeChanged -= UndoRecord;
            }
        }

        void Init()
        {
            RulesFoldout = true;
            PainterFoldout = true;
            PresetsFoldout = true;
            ShowGrid = Settings.ShowGrid;
            ShowWalkable = Settings.ShowWalkable;
        }

        void OnChangedMap()
        {
            if (Map)
            {
                GridType = (int)Map.Type;
                GridAxisType = (int)Map.Axis;
                RotationType = (int)Map.RotationType;
                foreach (var preset in Map.Presets)
                {
                    if (!preset.Validate())
                    {
                        EditorUtility.SetDirty(Map);
                    }
                }
            }
            SceneDrawer.Map = Map;
            SceneDrawer.Redraw();
        }

        void OnSceneGUI(SceneView sceneView)
        {
            SceneDrawer.Draw(IsSceneEditing);
            SceneView.RepaintAll();
        }

        void InitResources()
        {
            Settings = MapWindowSettings.Instance;
        }

        void OnUndoRedoPerformed()
        {
            Repaint();
        }

        void UndoRecord()
        {
            Undo.RegisterCompleteObjectUndo(this, "Map");
        }

        void RectDraw(ref Rect rect, float x, float y, float width, float height, Color color)
        {
            rect.x = x;
            rect.y = y;
            rect.width = width;
            rect.height = height;
            EditorGUI.DrawRect(rect, color);
        }

        void AreaCreator()
        {
            Undo.RecordObject(this, "Map");
            GUILayout.BeginArea(Creator);
            Map = EditorGUILayout.ObjectField("Map Asset", Map, typeof(MapSettings), allowSceneObjects: false) as MapSettings;
            GuiStyles.DrawHorizontal(Separator);

            EditorGUILayout.LabelField("Asset Creator", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Grid", GUILayout.Width(26));
            GridType = GUILayout.SelectionGrid(GridType, Grids, 3, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Axis", GUILayout.Width(26));
            GridAxisType = GUILayout.SelectionGrid(GridAxisType, GridAxes, 3, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Create", GUILayout.MaxWidth(MapWindowSettings.WindowMinSize.x - 6 * Border)))
            {
                MapCreateAsset();
            }
            GuiStyles.DrawHorizontal(Separator);
            GUILayout.EndArea();
        }

        void AreaRules()
        {
            GUILayout.BeginArea(Rules);
            RulesFoldout = EditorGUILayout.Foldout(RulesFoldout, "Rules", true);
            GUI.enabled = Map;
            if (RulesFoldout)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Rotation", GUILayout.Width(56));
                RotationType = GUILayout.SelectionGrid(RotationType, Rotations, 2, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();
                if (Map)
                {
                    Map.RotationType = (RotationType)RotationType;
                }
            }
            GUILayout.EndArea();
        }

        void AreaPainter()
        {
            GUILayout.BeginArea(Painter);
            GUI.enabled = GuiEnabled;
            PainterFoldout = EditorGUILayout.Foldout(PainterFoldout, "Painter", true);
            if (PainterFoldout)
            {
                GUILayout.BeginHorizontal();
                var backup = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 70f;
                ShowGrid = EditorGUILayout.Toggle("Show Grid", ShowGrid);
                EditorGUIUtility.labelWidth = 90f;
                ShowWalkable = EditorGUILayout.Toggle("Show Walkable", ShowWalkable);
                EditorGUIUtility.labelWidth = backup;
                GUILayout.EndHorizontal();
                GUI.enabled = Editable;
                if (!IsSceneEditingPrevious && IsSceneEditing)
                {
                    SceneEditingStart();
                }
                else if (IsSceneEditingPrevious && !IsSceneEditing)
                {
                    SceneEditingFinish();
                }

                GUILayout.BeginHorizontal();
                ToolToogle.CopyTo(ToolTooglePrevious, 0);
                DrawToolTiles();
                DrawToolEdges();
                GUILayout.EndHorizontal();

                GUI.enabled = GuiEnabled;
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                var maxWidth = GUILayout.MaxWidth((MapWindowSettings.WindowMinSize.x - 6 * Border) / 2f);
                if (GUILayout.Button("Clear", maxWidth))
                {
                    Action yes = () =>
                    {
                        Map.Clear();
                        SceneDrawer.Clear();
                    };
                    ConfirmEditorWindow.Init("Map data and scene\ndata will be cleared.\nContinue?", yesAction: yes);
                }
                if (GUILayout.Button("Place Prefabs", maxWidth))
                {
                    if (IsEmpty)
                    {
                        PlacePrefabs();
                    }
                    else
                    {
                        Action yes = () => { PlacePrefabs(); };
                        ConfirmEditorWindow.Init("MapView gameObject has childs.\nThey will be replaced.\nContinue?", yesAction: yes);
                    }
                }
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                if (GUILayout.Button("Areas Mark", maxWidth))
                {
                    MapUtils.MarkAreas(Map.Tiles, Map.Type, Map.Presets, Map.Rules);
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                for (int i = 0; i < ToolToogle.Length; i++)
                {
                    if (ToolToogle[i] && ToolToogle[i] != ToolTooglePrevious[i])
                    {
                        if (!ToolValidate())
                        {
                            for (int j = 0; j < ToolToogle.Length; j++)
                            {
                                ToolToogle[j] = false;
                            }
                            ConfirmEditorWindow.Init("Presets list is empty.\nPlease, add at least one preset", noText: "OK");
                            break;
                        }
                        ToolCurrent = i;
                        for (int j = 0; j < ToolToogle.Length; j++)
                        {
                            if (j != i)
                            {
                                ToolToogle[j] = false;
                            }
                        }
                        break;
                    }
                }
                SceneDrawer.ToolType = ToolCurrent == 0 || ToolCurrent == 2 ? 0 : 1;
                SceneDrawer.BrushType = ToolCurrent < 2 ? 0 : 1;
            }
            GUILayout.EndArea();
        }

        void AreaPresets()
        {
            GUILayout.BeginArea(Presets);
            GUI.enabled = GuiEnabled;
            PresetsFoldout = EditorGUILayout.Foldout(PresetsFoldout, "Presets", true);
            GUI.enabled = Editable;
            if (PresetsFoldout && Map)
            {
                var buttonAddStyle = Skin.customStyles[4];
                var buttonRemoveStyle = Skin.customStyles[5];
                var buttonTypeNormal = Skin.customStyles[9];
                var buttonTypeSelected = Skin.customStyles[10];
                var presetBackground = Skin.customStyles[11];
                ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);
                var labelWidth = EditorGUIUtility.labelWidth;

                var serializedMap = new SerializedObject(Map);
                var presetsProperty = serializedMap.FindProperty(nameof(Map.Presets));
                for (int i = 0; i < presetsProperty.arraySize; i++)
                {
                    var presetProperty = presetsProperty.GetArrayElementAtIndex(i);
                    var tagsProperty = presetProperty.FindPropertyRelative(nameof(TilePreset.Tags));
                    var prefabsProperty = presetProperty.FindPropertyRelative(nameof(TilePreset.Prefabs));
                    var isPresetSelected = TilePresetCurrent == i;
                    EditorGUIUtility.labelWidth = TileLabelWidth;

                    GUILayout.BeginHorizontal(presetBackground, GUILayout.Height(200f));

                    GUILayout.BeginVertical();

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(presetProperty.FindPropertyRelative(nameof(TilePreset.Type)), new GUIContent("Name"), true);
                    EditorGUILayout.LabelField("Prefabs");
                    EditorGUIUtility.labelWidth = 22f;
                    var elementWidth = 134f;
                    var column = Mathf.FloorToInt(Presets.width / elementWidth);
                    int counter = 0;
                    var run = true;
                    while (run)
                    {
                        GUILayout.BeginHorizontal();
                        for (int j = 0; j < column; j++)
                        {
                            if (counter == prefabsProperty.arraySize)
                            {
                                if (GUILayout.Button("+", buttonAddStyle, GUILayout.Height(90f), GUILayout.Width(90f)))
                                {
                                    prefabsProperty.arraySize++;
                                }
                                run = false;
                                break;
                            }
                            GUILayout.BeginVertical(GUILayout.Width(elementWidth));
                            GUILayout.BeginHorizontal();

                            var size = 100;
                            var prefabProperty = prefabsProperty.GetArrayElementAtIndex(counter);
                            var preview = GetPrefabPreview(prefabProperty.objectReferenceValue as GameObject, size, size);
                            if (preview == null)
                            {
                                var color = presetProperty.FindPropertyRelative(nameof(TilePreset.MapColor)).colorValue;
                                preview = new Texture2D(size, size);
                                for (int width = 0; width < size; width++)
                                {
                                    for (int height = 0; height < size; height++)
                                    {
                                        preview.SetPixel(width, height, color);
                                    }
                                }
                                preview.Apply();
                            }
                            if (ButtonTwoStyle(preview, buttonTypeNormal, buttonTypeSelected, isPresetSelected && TilePresetPrefab == counter, 100f))
                            {
                                TilePresetCurrent = i;
                                TilePresetPrefab = counter;
                            }

                            if (GUILayout.Button("-", buttonRemoveStyle, GUILayout.Width(24f)))
                            {
                                if (prefabProperty.objectReferenceValue)
                                {
                                    prefabProperty.DeleteCommand();
                                }
                                prefabProperty.DeleteCommand();
                                GUILayout.EndHorizontal();
                                break;
                            }
                            GUILayout.EndHorizontal();
                            EditorGUILayout.PropertyField(prefabProperty, new GUIContent(string.Empty), true);
                            GUILayout.EndVertical();
                            counter++;
                        }
                        GUILayout.EndHorizontal();
                    }

                    if (isPresetSelected)
                    {
                        EditorGUILayout.BeginVertical();
                        EditorGUIUtility.labelWidth = 100f;
                        EditorGUILayout.PropertyField(presetProperty.FindPropertyRelative(nameof(TilePreset.MapColor)), new GUIContent("Editor Color"), true);
                        EditorGUILayout.PropertyField(presetProperty.FindPropertyRelative(nameof(TilePreset.GridOffset)), new GUIContent("Grid Offset"), true);
                        EditorGUILayout.LabelField("Tags");
                        EditorGUIUtility.labelWidth = 22f;
                        for (int j = 0; j < tagsProperty.arraySize; j++)
                        {
                            GUILayout.BeginHorizontal();
                            var tagProperty = tagsProperty.GetArrayElementAtIndex(j);
                            EditorGUILayout.PropertyField(tagProperty, new GUIContent(" " + j + ":"), true);

                            if (GUILayout.Button("-", buttonRemoveStyle, GUILayout.Width(20f)))
                            {
                                if (tagProperty.objectReferenceValue)
                                {
                                    tagProperty.DeleteCommand();
                                }
                                tagProperty.DeleteCommand();
                                GUILayout.EndHorizontal();
                                break;
                            }
                            GUILayout.EndHorizontal();
                        }
                        EditorGUIUtility.labelWidth = labelWidth;
                        if (GUILayout.Button("+", buttonAddStyle, GUILayout.Height(20f)))
                        {
                            tagsProperty.arraySize++;
                        }
                        GUILayout.EndVertical();
                    }
                    EditorGUIUtility.labelWidth = labelWidth;
                    GUILayout.EndVertical();

                    if (GUILayout.Button("-", buttonRemoveStyle, GUILayout.Width(20f)))
                    {
                        Map.PresetRemove(i);
                        SceneDrawer.Redraw();
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                        break;
                    }

                    GUILayout.EndHorizontal();
                }

                if (GUILayout.Button("+", buttonAddStyle, GUILayout.Height(20)))
                {
                    Map.PresetAddDefault();
                    if (IsSceneEditing)
                    {
                        SceneDrawer.Redraw();
                    }
                }
                serializedMap.ApplyModifiedProperties();

                TilePresetCurrent = Mathf.Clamp(TilePresetCurrent, 0, Map.Presets.Count - 1);
                if (0 <= TilePresetCurrent && TilePresetCurrent < Map.Presets.Count)
                {
                    TilePresetPrefab = Mathf.Clamp(TilePresetPrefab, 0, Map.Presets[TilePresetCurrent].Prefabs.Count - 1);
                }
                EditorGUIUtility.labelWidth = labelWidth;

                SceneDrawer.TileType = TilePresetCurrent;
                SceneDrawer.TilePrefabIndex = TilePresetPrefab;

                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        void DrawToolTiles()
        {
            var toolStyleNormal = Skin.customStyles[7];
            var toolStyleSelected = Skin.customStyles[8];

            GUILayout.BeginVertical();
            EditorGUILayout.LabelField("Tile", EditorStyles.boldLabel, GUILayout.Width(40f));
            GUILayout.BeginHorizontal();
            if (ButtonTwoStyle(Settings.BrushIcon, TileToolNames[0], toolStyleNormal, toolStyleSelected, ToolToogle[0]))
            {
                ToolToogle[0] = !ToolToogle[0];
            }

            if (ButtonTwoStyle(Settings.EraseIcon, TileToolNames[1], toolStyleNormal, toolStyleSelected, ToolToogle[1]))
            {
                ToolToogle[1] = !ToolToogle[1];
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        void DrawToolEdges()
        {
            if (!Settings.DrawSideTool)
            {
                return;
            }
            var toolStyleNormal = Skin.customStyles[7];
            var toolStyleSelected = Skin.customStyles[8];

            GUILayout.BeginVertical();
            EditorGUILayout.LabelField("Edge", EditorStyles.boldLabel, GUILayout.Width(40f));
            GUILayout.BeginHorizontal();
            var labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = ToolLabelWidth;

            if (ButtonTwoStyle(Settings.BrushIcon, EdgeToolNames[0], toolStyleNormal, toolStyleSelected, ToolToogle[2]))
            {
                ToolToogle[2] = !ToolToogle[2];
            }
            if (ButtonTwoStyle(Settings.EraseIcon, EdgeToolNames[1], toolStyleNormal, toolStyleSelected, ToolToogle[3]))
            {
                ToolToogle[3] = !ToolToogle[3];
            }
            EditorGUIUtility.labelWidth = labelWidth;
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        void SceneEditingStart()
        {
            SceneDrawer.Map = Map;
            SceneDrawer.Redraw();
        }

        void SceneEditingFinish()
        {
            SceneDrawer.Release();
            MapUtils.MarkAreas(Map.Tiles, Map.Type, Map.Presets, Map.Rules);
        }

        void MapCreateAsset()
        {
            var scene = EditorSceneManager.GetActiveScene();
            var filename = System.IO.Path.GetFileNameWithoutExtension(scene.path);
            var directory = System.IO.Path.GetDirectoryName(scene.path);
            var mapPath = MapSettings.Path(directory, filename, Grids[GridType]);
            mapPath = AssetDatabase.GenerateUniqueAssetPath(mapPath);
            var mapInstance = ScriptableObject.CreateInstance<MapSettings>();
            mapInstance.Init((GridType)GridType, (GridAxis)GridAxisType, Settings.Rules, Settings.CellBorder);
            AssetDatabase.CreateAsset(mapInstance, mapPath);
            AssetDatabase.SaveAssets();
            Map = mapInstance;
        }

        bool ToolValidate()
        {
            return Map.Presets != null && Map.Presets.Count > 0;
        }

        bool ButtonTwoStyle(Texture2D icon, string tooltip, GUIStyle normal, GUIStyle selected, bool state)
        {
            var toolPressed = false;
            if (state)
            {
                toolPressed = GUILayout.Button(new GUIContent(icon, tooltip), selected, GUILayout.Width(32f), GUILayout.Height(32f));
            }
            else
            {
                toolPressed = GUILayout.Button(new GUIContent(icon, tooltip), normal, GUILayout.Width(32f), GUILayout.Height(32f));
            }
            return toolPressed;
        }

        bool ButtonTwoStyle(Texture2D icon, GUIStyle normal, GUIStyle selected, bool state, float width)
        {
            GUIStyle style = state ? selected : normal;
            return GUILayout.Button(icon, style, GUILayout.Width(width), GUILayout.Height(width)); ;
        }

        /// <summary>
        /// Place tile presets prefabs to scene
        /// </summary>
        public void PlacePrefabs()
        {
            var holder = GetMapHolder(true);
            var parent = new GameObject("Tiles");
            parent.transform.SetParent(holder.transform);
            parent.transform.localPosition = Vector3.zero;
            foreach (var tile in Map.Tiles)
            {
                var preset = Map.Presets.FirstOrDefault(p => p.Id == tile.Id);
                if (preset != null)
                {
                    GameObject prefab = null;
                    if (tile.PrefabIndex >= preset.Prefabs.Count)
                    {
                        tile.PrefabIndex = Mathf.Max(0, preset.Prefabs.Count - 1);
                    }
                    if (tile.PrefabIndex < preset.Prefabs.Count)
                    {
                        prefab = preset.Prefabs[tile.PrefabIndex];
                    }
                    if (prefab != null)
                    {
#if UNITY_EDITOR
                        var tileGo = PrefabUtility.InstantiatePrefab(prefab, parent.transform) as GameObject;
                        tileGo.transform.localRotation = Quaternion.Inverse(parent.transform.rotation);
                        tileGo.transform.position = Map.ToWorld(tile.TilePos, Map.Edge);
                        Undo.RegisterCreatedObjectUndo(tileGo, "Create MapView");
#endif
                    }
                }
            }
#if UNITY_EDITOR
            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
#endif
        }

        MapView GetMapHolder(bool doClean = false)
        {
#if UNITY_EDITOR
#if UNITY_2023_1_OR_NEWER
            var holder = GameObject.FindFirstObjectByType<MapView>();
#else
            var holder = GameObject.FindObjectOfType<MapView>();
#endif
            if (holder == null)
            {
                var holderGo = new GameObject();
                holderGo.name = "MapView";
                holderGo.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                holder = holderGo.AddComponent<MapView>();
                Undo.RegisterCreatedObjectUndo(holderGo, "Create Map View");
            }

            if (doClean)
            {
                for (int i = holder.transform.childCount - 1; i >= 0; i--)
                {
                    Undo.DestroyObjectImmediate(holder.transform.GetChild(i).gameObject);
                }
            }
            return holder;
#else
            return null;
#endif
        }

        public static MapView CreateHolder()
        {
            var holderGo = new GameObject();
            holderGo.name = "MapView";
            holderGo.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            var holder = holderGo.AddComponent<MapView>();
            return holder;
        }

        /// <summary>
        /// Map View at Scene View is empty?
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                var holder = GetMapHolder();
                return holder == null || holder.transform.childCount == 0;
            }
        }

        Texture2D GetPrefabPreview(GameObject prefab, int width = 100, int height = 100)
        {
            Texture2D texture = null;
            if (prefab)
            {
                var editor = Editor.CreateEditor(prefab);
                texture = editor.RenderStaticPreview(AssetDatabase.GetAssetPath(prefab), null, width, height);
                EditorWindow.DestroyImmediate(editor);
            }
            return texture;
        }
    }
}
