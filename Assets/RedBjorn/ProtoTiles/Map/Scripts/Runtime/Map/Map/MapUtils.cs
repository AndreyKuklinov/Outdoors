using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    public class MapUtils
    {
        public static Func<Vector3Int, Vector3Int, float> DistanceFunc(GridType grid)
        {
            switch (grid)
            {
                case GridType.HexFlat: return Hex.Distance;
                case GridType.HexPointy: return Hex.Distance;
                case GridType.Square: return Square.Distance;
            }
            return null;
        }

        /// <summary>
        /// Neighbour directions in integer coordinates with side size = 1
        /// </summary>
        public static Vector3Int[] NeighbourDirections(GridType grid)
        {
            switch (grid)
            {
                case GridType.HexFlat: return Hex.Neighbour;
                case GridType.HexPointy: return Hex.Neighbour;
                case GridType.Square: return Square.Neighbour;
            }
            return null;
        }

        /// <summary>
        /// Calculate tile area ownership
        /// </summary>
        public static void MarkAreas(List<TileData> tiles, GridType grid, List<TilePreset> presets, MapRules rules)
        {
            var ditanceFunc = DistanceFunc(grid);
            var neighbours = NeighbourDirections(grid);
            var mapMock = new MapEntityMock();
            mapMock.DistanceFunc = ditanceFunc;
            mapMock.NeighboursDirection = neighbours;
            for (int i = 0; i < tiles.Count; i++)
            {
                var tilePreset = tiles[i];
                var type = presets.FirstOrDefault(t => t.Id == tilePreset.Id);
                mapMock.Tiles[tiles[i].TilePos] = new TileEntity(tilePreset, type, rules);
            }
            int movableArea = 1;
            var marked = new HashSet<INode>();
            var walkableCount = mapMock.Tiles.Count(t => t.Value.Vacant);
            while (marked.Count < walkableCount)
            {
                var walkable = mapMock.Tiles.FirstOrDefault(t => t.Value.Vacant && !marked.Any(m => m.Position == t.Value.Position));
                if (walkable.Value != null)
                {
                    var accessible = NodePathFinder.AccessibleArea(mapMock, walkable.Value);
                    foreach (var a in accessible)
                    {
                        a.ChangeMovableAreaPreset(movableArea);
                        marked.Add(a);
                    }
                }
                else
                {
                    break;
                }
                movableArea++;
            }

            foreach (var t in mapMock.Tiles.Where(t => !t.Value.Vacant))
            {
                t.Value.Data.MovableArea = 0;
            }
        }
    }
}
