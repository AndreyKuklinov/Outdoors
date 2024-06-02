using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileRaycaster : MonoBehaviour
{
    [SerializeField] private  Tilemap tilemap;

    public Vector3Int GetTilePosition(Vector3 origin)
    {
        // var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(origin), Camera.main.transform.forward);
        // var tilePos = tilemap.WorldToCell(hit.point);
        // return tilePos;

        var worldPos = Camera.main.ScreenToWorldPoint(origin);
        var cellPos = tilemap.WorldToCell(worldPos);
        return cellPos;
    }
}
