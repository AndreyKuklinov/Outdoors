using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogManager : MonoBehaviour
{
    [SerializeField] Tilemap _tileMap;
    [SerializeField] Tile[] _fogTiles;
    [SerializeField] Camera _camera;
    [SerializeField] CameraDrag _cameraDrag;
    [SerializeField] int _generationRadius;   

    void Start()
    {
        GenerateTilesAroundCamera();
        _cameraDrag.OnCameraDrag += GenerateTilesAroundCamera;
    }

    void GenerateTilesAroundCamera()
    {
        var cameraPosition = _camera.transform.position;
        var tilePosition = _tileMap.WorldToCell(cameraPosition);

        for(var x = tilePosition.x - _generationRadius; x <= tilePosition.x + _generationRadius; x++)
        {
            for(var y = tilePosition.y - _generationRadius; y <= tilePosition.y + _generationRadius; y++)
            {
                _tileMap.SetTile(new Vector3Int(x, y, 0), PickRandomFogTile());
            }
        }
    }

    Tile PickRandomFogTile()
        => _fogTiles[Random.Range(0, _fogTiles.Length)];
}
