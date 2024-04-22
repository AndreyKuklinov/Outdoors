using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    [Serializable]
    public class MapSceneDrawer
    {
        [Serializable]
        public struct SideInfo
        {
            public Vector3Int TilesPos;
            public int NeighbourIndex;

            public override int GetHashCode()
            {
                int hash = 13;
                hash = (7 * hash) + NeighbourIndex.GetHashCode();
                hash = (7 * hash) + TilesPos.GetHashCode();
                return hash;
            }
        }

        [Serializable]
        public class TilePresetDictionary : SerializableDictionary<Vector3Int, TileData> { } // Hack to serialize dictionary
        [Serializable]
        public class TileColorsDictionary : SerializableDictionary<string, Color> { } // Hack to serialize dictionary
        [Serializable]
        public class CursorTilesColorsDictionary : SerializableDictionary<string, Color> { } // Hack to serialize dictionary

        public MapSettings Map;
        public MapWindow Window;
        public MapWindowSettings WindowSettings;

        [SerializeField]
        bool IsEditing;
        [SerializeField]
        bool MouseDown;
        [SerializeField]
        bool MouseUp;
        [SerializeField]
        Ray MouseRay;
        [SerializeField]
        TilePresetDictionary Tiles = new TilePresetDictionary();
        [SerializeField]
        TileColorsDictionary TileColors = new TileColorsDictionary();
        [SerializeField]
        CursorTilesColorsDictionary CursorTileColors = new CursorTilesColorsDictionary();

        Material CursorMaterial2D;
        Material CursorMaterial3D;
        Material InstanceMaterial2D;
        Material InstanceMaterial3D;
        GameObject CursorPrefab;
        GameObject CursorInstance;
        GameObject TilesDraftParent;
        HashSet<Vector3Int> TilesPotential = new HashSet<Vector3Int>();
        HashSet<Vector3Int> TilesDraft = new HashSet<Vector3Int>();
        HashSet<SideInfo> EdgesDraft = new HashSet<SideInfo>();

        static Plane GroundXZ = new Plane(Vector3.up, Vector3.zero);
        static Plane GroundXY = new Plane(Vector3.back, Vector3.zero);
        static Plane GroundZY = new Plane(Vector3.right, Vector3.zero);

        static Plane Ground(GridAxis axis)
        {
            switch (axis)
            {
                case GridAxis.XZ: return GroundXZ;
                case GridAxis.XY: return GroundXY;
                case GridAxis.ZY: return GroundZY;
            }
            return new Plane();
        }

        Color EdgeCursorColor => WindowSettings.EdgeCursorColor;
        Color EdgeCursorPaint => WindowSettings.EdgeCursorPaint;
        Color EdgeCursorErase => WindowSettings.EdgeCursorErase;

        int CachedBrushType;
        public int BrushType
        {
            get
            {
                return CachedBrushType;
            }
            set
            {
                if (value != CachedBrushType)
                {
                    CachedBrushType = value;
                    OnBrushSwitched();
                }
                else
                {
                    CachedBrushType = value;
                }
            }
        }

        void OnBrushSwitched()
        {
            if (CachedBrushType == 0)
            {
                EdgesDraft.Clear();
            }
            else if (CachedBrushType == 1)
            {
                IsEditing = false;
                TilesDraft.Clear();
                TilesPotential.Clear();
            }
        }

        int CachedToolType;
        public int ToolType
        {
            get
            {
                return CachedToolType;
            }
            set
            {
                if (value != CachedToolType)
                {
                    CachedToolType = value;
                }
                else
                {
                    CachedToolType = value;
                }
            }
        }

        int CachedTileType;
        public int TileType
        {
            get
            {
                return CachedTileType;
            }
            set
            {
                CachedTileType = value;
            }
        }

        public int TilePrefabIndex;

        public event Action OnBeforeChanged;

        public void Release()
        {
            DrawFinish();
            GameObject.DestroyImmediate(CursorInstance);
            GameObject.DestroyImmediate(TilesDraftParent);
            TilesDraftParent = null;
            CursorInstance = null;
            CursorPrefab = null;
        }

        public void Clear()
        {
            IsEditing = false;
            Tiles.Clear();
            TileColors.Clear();
            CursorTileColors.Clear();
            TilesPotential.Clear();
            TilesDraft.Clear();
            EdgesDraft.Clear();
        }

        public void Redraw()
        {
            Clear();
            if (Map != null)
            {
                foreach (var p in Map.Presets)
                {
                    TileColors.Add(p.Id, p.MapColor);
                    CursorTileColors.Add(p.Id, new Color(p.MapColor.r, p.MapColor.g, p.MapColor.b, Mathf.Clamp01(p.MapColor.a * 1.5f)));
                }

                foreach (var t in Map.Tiles)
                {
                    Tiles.Add(t.TilePos, t);
                }
            }
        }

        public void Draw(bool isDrawing)
        {
            if (Map != null)
            {
                System.Action<Vector3, Color, float, GridAxis> drawTile = null;
                if (Window.ShowGrid)
                {
                    if (Map.Type == GridType.HexFlat)
                    {
                        drawTile = GuiStyles.DrawHexFlat;
                    }
                    else if (Map.Type == GridType.HexPointy)
                    {
                        drawTile = GuiStyles.DrawHexPointy;
                    }
                    else if (Map.Type == GridType.Square)
                    {
                        drawTile = GuiStyles.DrawSquare;
                    }
                }

                foreach (var t in Tiles)
                {
                    var pos = Map.ToWorld(t.Key, Map.Edge);
                    if (drawTile != null && !string.IsNullOrEmpty(t.Value.Id))
                    {
                        var color = TileColors.TryGetOrDefault(t.Value.Id);
                        drawTile(pos, color, Map.Edge, Map.Axis);
                    }
                    if (Window.ShowWalkable)
                    {
                        GuiStyles.DrawLabel(t.Value.MovableArea.ToString(), pos, WindowSettings.LabelColor);
                    }
                }
            }

            if (isDrawing)
            {
                if (CachedBrushType == 0)
                {
                    TileBrushDraw();
                }
                else if (CachedBrushType == 1)
                {
                    EdgesBrushDraw();
                }
            }
        }

        void DrawFinish()
        {
            IsEditing = false;
            TilesPotential.Clear();
            TilesDraft.Clear();
            EdgesDraft.Clear();
            if (Map)
            {
                if (Tiles != null)
                {
                    Map.Tiles = Tiles.Select(x => x.Value).ToList();
                }
                EditorUtility.SetDirty(Map);
            }
        }

        void TileBrushDraw()
        {
            if (Map == null)
            {
                return;
            }
            Validate();
            var ev = Event.current;
            MouseUpdate(ev);

            float enter;
            var positionGround = Vector3.zero;
            if (Ground(Map.Axis).Raycast(MouseRay, out enter))
            {
                positionGround = MouseRay.GetPoint(enter);
            }
            var positionTile = Map.ToTile(positionGround, Map.Edge);
            var positionWorld = Map.ToWorld(positionTile, Map.Edge);

            EditStart();
            EditFinish();
            EditUpdate(positionTile);
            CursorUpdate(positionWorld);

            EventHandle(ev);
        }

        void MouseUpdate(Event ev)
        {
            MouseDown = false;
            MouseUp = false;
            if (ev.type == EventType.MouseDown)
            {
                if (ev.button == 0 && !ev.alt && !ev.shift)
                {
                    MouseDown = true;
                }
            }
            else if (ev.type == EventType.MouseUp)
            {
                if (ev.button == 0)
                {
                    MouseUp = true;
                }
            }

            MouseRay = HandleUtility.GUIPointToWorldRay(ev.mousePosition);
        }

        void EditStart()
        {
            if (MouseDown)
            {
                IsEditing = true;
                TilesPotential.Clear();
                TilesDraft.Clear();
                if (!TilesDraftParent)
                {
                    TilesDraftParent = new GameObject("Draft");
                    TilesDraftParent.transform.position = Vector3.zero;
                }
            }
        }

        void EditFinish()
        {
            if (MouseUp)
            {
                IsEditing = false;
                OnBeforeChanged.SafeInvoke();
                foreach (var tile in TilesDraft)
                {
                    var existed = Tiles.TryGetOrDefault(tile);
                    if (ToolType == 0)
                    {
                        if (existed == null)
                        {
                            existed = new TileData { TilePos = tile };
                            Tiles.Add(tile, existed);
                        }
                        var preset = Map.Presets[CachedTileType];
                        existed.Id = preset.Id;
                        existed.PrefabIndex = TilePrefabIndex;
                    }
                    else if (ToolType == 1)
                    {
                        if (existed != null)
                        {
                            Tiles.Remove(tile);
                        }
                    }
                }
                if (TilesDraftParent)
                {
                    for (int i = TilesDraftParent.transform.childCount - 1; i >= 0; i--)
                    {
                        GameObject.DestroyImmediate(TilesDraftParent.transform.GetChild(i).gameObject);
                    }
                }
                TilesDraft.Clear();
                TilesPotential.Clear();
                DrawFinish();
                if (WindowSettings.AreasAutoMark)
                {
                    MapUtils.MarkAreas(Map.Tiles, Map.Type, Map.Presets, Map.Rules);
                }
                Window.PlacePrefabs();
            }
        }

        void EditUpdate(Vector3Int position)
        {
            if (IsEditing)
            {
                TilesPotential.Add(position);
                if (ToolType == 0)
                {
                    var preset = Map.Presets[CachedTileType];
                    if (CursorPrefab)
                    {
                        foreach (var tilePosition in TilesPotential)
                        {
                            if (TilesDraft.Add(tilePosition))
                            {
                                var worldPosition = Map.ToWorld(tilePosition, Map.Edge);
                                var instance = GameObject.Instantiate(CursorPrefab);
                                instance.transform.position = worldPosition;
                                instance.transform.SetParent(TilesDraftParent.transform);
                                InstanceMaterial2D.SetColor("_Color", preset.MapColor);
                                InstanceMaterial3D.SetColor("_Color", preset.MapColor);
                                foreach (var r in instance.GetComponentsInChildren<SpriteRenderer>())
                                {
                                    r.sharedMaterial = InstanceMaterial2D;
                                    r.sortingOrder = 32767;
                                }
                                foreach (var r in instance.GetComponentsInChildren<MeshRenderer>())
                                {
                                    r.sharedMaterial = InstanceMaterial3D;
                                }
                            }
                        }
                        TilesPotential.Clear();
                    }
                    else
                    {
                        foreach (var tile in TilesPotential)
                        {
                            TilesDraft.Add(tile);
                        }
                        TilesPotential.Clear();
                        TilesDraftDraw(preset.MapColor);
                    }
                }

                if (ToolType == 1)
                {
                    foreach (var tilePosition in TilesPotential)
                    {
                        if (TilesDraft.Add(tilePosition))
                        {

                        }
                    }
                    TilesPotential.Clear();
                    TilesDraftDraw(WindowSettings.TileCursorErase);
                }
            }
        }

        void CursorUpdate(Vector3 position)
        {
            if (ToolType == 0)
            {
                if (CursorInstance)
                {
                    CursorInstance.transform.position = position;
                    if (!CursorInstance.activeSelf)
                    {
                        CursorInstance.SetActive(true);
                    }
                }
                else
                {
                    TileDraw(position, WindowSettings.TileCursorBrush, Map.Edge);
                }
            }

            if (ToolType == 1)
            {
                TileDraw(position, WindowSettings.TileCursorErase, Map.Edge);
                if (CursorInstance && CursorInstance.activeSelf)
                {
                    CursorInstance.SetActive(false);
                }
            }
        }

        void EventHandle(Event ev)
        {
            if (ev.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(0);
            }
        }

        void TileDraw(Vector3 point, Color color, float edge)
        {
            System.Action<Vector3, Color, float, GridAxis> drawTile = null;
            if (Map.Type == GridType.HexFlat)
            {
                drawTile = GuiStyles.DrawHexFlat;
            }
            else if (Map.Type == GridType.HexPointy)
            {
                drawTile = GuiStyles.DrawHexPointy;
            }
            else if (Map.Type == GridType.Square)
            {
                drawTile = GuiStyles.DrawSquare;
            }
            if (drawTile != null)
            {
                drawTile(point, color, edge, Map.Axis);
            }
        }

        void TilesDraftDraw(Color color)
        {
            System.Action<Vector3, Color, float, GridAxis> drawTile = null;
            if (Map.Type == GridType.HexFlat)
            {
                drawTile = GuiStyles.DrawHexFlat;
            }
            else if (Map.Type == GridType.HexPointy)
            {
                drawTile = GuiStyles.DrawHexPointy;
            }
            else if (Map.Type == GridType.Square)
            {
                drawTile = GuiStyles.DrawSquare;
            }

            if (drawTile != null)
            {
                foreach (var t in TilesDraft)
                {
                    drawTile(Map.ToWorld(t, Map.Edge), color, Map.Edge, Map.Axis);
                }
            }
        }

        void Validate()
        {
            if (!InstanceMaterial2D)//
            {
                InstanceMaterial2D = new Material(WindowSettings.ShaderDraw2D);
            }

            if (!InstanceMaterial3D)
            {
                InstanceMaterial3D = new Material(WindowSettings.ShaderDraw3D);
            }

            if (!CursorMaterial2D)
            {
                CursorMaterial2D = new Material(WindowSettings.ShaderDraw2D);
                CursorMaterial2D.SetColor("_Color", new Color(0f, 1f, 0f, 0.7f));
            }

            if (!CursorMaterial3D)
            {
                CursorMaterial3D = new Material(WindowSettings.ShaderDraw3D);
                CursorMaterial3D.SetColor("_Color", new Color(0f, 1f, 0f, 0.7f));
            }

            var preset = 0 <= CachedTileType && CachedTileType < Map.Presets.Count ? Map.Presets[CachedTileType] : null;
            var prefab = preset != null && 0 <= TilePrefabIndex && TilePrefabIndex < preset.Prefabs.Count ? preset.Prefabs[TilePrefabIndex] : null;

            if (CursorPrefab != prefab)
            {
                GameObject.DestroyImmediate(CursorInstance);
                CursorPrefab = prefab;
                if (CursorPrefab)
                {
                    CursorInstance = GameObject.Instantiate(CursorPrefab);
                    CursorInstance.name = "Cursor";
                    foreach (var r in CursorInstance.GetComponentsInChildren<SpriteRenderer>())
                    {
                        r.sharedMaterial = CursorMaterial2D;
                        r.sortingOrder = 32767;
                    }

                    foreach (var r in CursorInstance.GetComponentsInChildren<MeshRenderer>())
                    {
                        r.sharedMaterial = CursorMaterial3D;
                    }
                }
            }
        }
        #region TODO
        void EdgesBrushDraw()
        {
            if (Map != null)
            {
                var ev = Event.current;
                var ray = HandleUtility.GUIPointToWorldRay(ev.mousePosition);

                float enter;
                var groundPos = Vector3.zero;
                if (Ground(Map.Axis).Raycast(ray, out enter))
                {
                    groundPos = ray.GetPoint(enter);
                }

                var tilePos = Map.ToTile(groundPos, Map.Edge);
                var worldTileCenter = Map.TileCenterWorld(groundPos);
                var neighbourIndex = Map.TileNeighbourIndexAtDirection(groundPos - worldTileCenter);

                var neighPos = worldTileCenter + Map.ToWorld(Map.TileNeighbourAtIndex(neighbourIndex), Map.Edge);
                var edgePos = (worldTileCenter + neighPos) / 2f;
                var edgeRot = Map.TileSideRotation(neighbourIndex);


                if (ev.type == EventType.MouseDown)
                {
                    if (ev.button == 0 && !ev.alt && !ev.shift)
                    {
                        EdgesDraft.Clear();
                        EdgesDraft.Add(new SideInfo { TilesPos = tilePos, NeighbourIndex = neighbourIndex });
                    }
                }
                else if (ev.type == EventType.MouseUp)
                {
                    if (ev.button == 0)
                    {
                        EdgesTempApply();
                    }
                }
                if (EdgesDraft.Any())
                {
                    EdgesDraft.Add(new SideInfo { TilesPos = tilePos, NeighbourIndex = neighbourIndex });
                }

                EdgesTempDraw();
                EdgesCursorShow(edgePos, EdgeCursorColor, edgeRot, Map.Edge);

                if (Event.current.type == EventType.Layout)
                {
                    HandleUtility.AddDefaultControl(0);
                }
            }
        }

        void EdgesCursorShow(Vector3 point, Color color, float angle, float edge)
        {
            GuiStyles.DrawRect(point, color, 90f + angle, edge, 0.1f * edge, Map.Axis);
            Handles.Label(point, string.Format("Edges = {0}", EdgesDraft.Count.ToString()), GuiStyles.CenterAligment);
        }

        void EdgesTempApply()
        {
            OnBeforeChanged.SafeInvoke();
            var obstacleHeight = CachedToolType == 0 ? 1f : 0f;
            foreach (var t in EdgesDraft)
            {
                var existed = Tiles.TryGetOrDefault(t.TilesPos);
                if (existed != null)
                {
                    existed.SideHeight[t.NeighbourIndex] = obstacleHeight;
                }
                var neighbour = Tiles.TryGetOrDefault(t.TilesPos + Map.TileNeighbourAtIndex(t.NeighbourIndex));
                if (neighbour != null)
                {
                    neighbour.SideHeight[Map.TileNeighbourIndexOpposite(t.NeighbourIndex)] = obstacleHeight;
                }
            }
            EdgesDraft.Clear();
            DrawFinish();
            if (WindowSettings.AreasAutoMark)
            {
                MapUtils.MarkAreas(Map.Tiles, Map.Type, Map.Presets, Map.Rules);
            }
        }

        void EdgesTempDraw()
        {
            var color = ToolType == 0 ? EdgeCursorPaint : EdgeCursorErase;
            foreach (var t in EdgesDraft)
            {
                var tile = Map.GetTile(t.TilesPos);
                if (tile != null)
                {
                    var obstacleHeight = tile.SideHeight[t.NeighbourIndex];
                    var worldPos = Map.ToWorld(t.TilesPos, Map.Edge);
                    var neighPos = worldPos + Map.ToWorld(Map.TileNeighbourAtIndex(t.NeighbourIndex), Map.Edge);
                    var edgePos = (worldPos + neighPos) / 2f;
                    GuiStyles.DrawRect(edgePos, color, Map.TileSideRotation(t.NeighbourIndex) + 90f, Map.Edge, 0.1f * Map.Edge, Map.Axis);
                }
            }
        }
        #endregion // TODO
    }
}
