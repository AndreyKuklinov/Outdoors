using System;
using System.Linq;
using RedBjorn.ProtoTiles;
using RedBjorn.ProtoTiles.Example;
using UnityEngine;
using Random = System.Random;

public class GridManager : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private MapSettings mapSettings;
    [SerializeField] private MapView mapView;
    [SerializeField] private Transform mapParent;
    [SerializeField] private GameState gameState;
    
    private MapEntity _mapEntity;
    private Random _random;
    private Array _tileTypes;

    private static Vector3Int[] _neighborCoordinates = new[]
    {
        new Vector3Int(-1, 1, 0),
        new Vector3Int(1, -1, 0),
        new Vector3Int(-1, 0, 1),
        new Vector3Int(1, 0, -1),
        new Vector3Int(0, -1, 1),
        new Vector3Int(0, 1, -1),
    };

    #endregion

    #region Private Methods
    
    private void UnlockNeighboringTiles(Vector3Int coordinates)
    {
        foreach (var shift in _neighborCoordinates)
        {
            var newCoordinates = coordinates + shift;
            if (_mapEntity.Tile(newCoordinates) != null)
                continue;
            CreateTile(newCoordinates);
        }
    }

    private void CreateTile(Vector3Int coordinates)
    {
        var tileType = GetRandomTileType();
        var typeId = ((int)tileType).ToString();
        var tilePreset = new TileData 
            { TilePos = coordinates, TileType = tileType, Id = typeId };
        mapSettings.Tiles.Add(tilePreset);
        var type = mapSettings.Presets.FirstOrDefault(preset => preset.Id == typeId);
        _mapEntity.InsertTile(tilePreset, type);
        
        if (type == null)
            throw new NullReferenceException("Tile preset not found");
        var newTile = Instantiate(type.Prefab, mapParent);
        newTile.transform.position = _mapEntity.WorldPosition(coordinates);
    }

    private TileType GetRandomTileType()
    {
        return (TileType)_tileTypes.GetValue(_random.Next(_tileTypes.Length));
    }

    private void ProcessTileClick(TileEntity tile)
    {
        UnlockNeighboringTiles(tile.Position);
        gameState.AddResource(tile.TileType);
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
                ProcessTileClick(tile);
            }
        }
    }

    #endregion
}