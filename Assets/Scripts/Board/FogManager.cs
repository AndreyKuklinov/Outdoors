using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogManager : MonoBehaviour
{
    [SerializeField] Tilemap _tileMap;
    [SerializeField] Tile _fogTile;
    [SerializeField] Camera _camera;
    [SerializeField] int _generationRadius;   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GenerateTilesAroundCamera();
    }

    void GenerateTilesAroundCamera()
    {
        var cameraPosition = _camera.transform.position;
        var tilePosition = _tileMap.WorldToCell(cameraPosition);

        for(var x = tilePosition.x - _generationRadius; x <= tilePosition.x + _generationRadius; x++)
        {
            for(var y = tilePosition.y - _generationRadius; y <= tilePosition.y + _generationRadius; y++)
            {
                _tileMap.SetTile(new Vector3Int(x, y, 0), _fogTile);
            }
        }
    }
}
