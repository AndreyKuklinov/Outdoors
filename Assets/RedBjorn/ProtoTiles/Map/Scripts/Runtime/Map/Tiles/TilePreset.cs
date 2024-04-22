using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    [Serializable]
    public class TilePreset
    {
        public string Type;
        public string Id;
        public Color MapColor;
        public GameObject Prefab;
        public float GridOffset;
        public List<TileTag> Tags = new List<TileTag>();
        public List<GameObject> Prefabs = new List<GameObject>();

        public GameObject PrefabCurrent => Prefabs == null || Prefabs.Count == 0 ? null : Prefabs[0];

        public bool Validate()
        {
            var valid = true;
            if (Prefabs == null)
            {
                Prefabs = new List<GameObject>();
                valid = false;
            }
            if (Prefab == null)
            {
                if (Prefabs.Count < 1)
                {
                    Prefabs.Add(null);
                    valid = false;
                }
            }
            else 
            {
                if (Prefabs.Count < 1)
                {
                    Prefabs.Add(null);
                    valid = false;
                }
                if (Prefabs[0] != Prefab)
                {
                    Prefabs[0] = Prefab;
                    Prefab = null;
                    valid = false;
                }
            }
            return valid;
        }
    }
}