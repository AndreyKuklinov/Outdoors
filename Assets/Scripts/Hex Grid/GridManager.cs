using System;
using System.Linq;
using RedBjorn.ProtoTiles;
using RedBjorn.ProtoTiles.Example;
using UnityEngine;
using Random = System.Random;

public class GridManager : MonoBehaviour
{
    [SerializeField] private MapSettings mapSettings;
    [SerializeField] private MapView mapView;
    
    private MapEntity _mapEntity;
    private Random _random;
    private Array _tileTypes;

    #region Private Methods

    private void CreateTile(Vector3Int coordinates)
    {
        var tileType = GetRandomTileType();
        var tilePreset = new TileData(coordinates, tileType);
        mapSettings.Tiles.Add(tilePreset);
        var type = mapSettings.Presets.FirstOrDefault(t => t.Id == tilePreset.Id);
        _mapEntity.InsertTile(tilePreset, type);
    }

    private TileType GetRandomTileType()
    {
        return (TileType)_tileTypes.GetValue(_random.Next(_tileTypes.Length));
    }

    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        if (!mapView || !mapSettings)
            throw new NullReferenceException("One or more fields not set");
        
        _mapEntity = new MapEntity(mapSettings, mapView);
        mapView.Init(_mapEntity);
        _random = new Random();
        _tileTypes = Enum.GetValues(typeof(TileType));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var clickPos = MyInput.GroundPosition(_mapEntity.Settings.Plane());
            var tile = _mapEntity.Tile(clickPos);
            if (tile != null)
            {
                Debug.Log(tile);
            }
        }
    }

    #endregion
}