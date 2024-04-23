using System;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    [Serializable]
    public class TileData
    {
        public Vector3Int TilePos;
        public string Id;
        public int PrefabIndex;
        public int MovableArea;
        public float[] SideHeight = new float[6] { 0f, 0f, 0f, 0f, 0f, 0f };
        public TileType TileType;

        public TileData(Vector3Int tilePos, TileType tileType, int prefabIndex = 0, string id = "")
        {
            TilePos = tilePos;
            TileType = tileType;
            PrefabIndex = prefabIndex;
            Id = id;
        }
    }
}