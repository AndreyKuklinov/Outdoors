using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private ScriptableTileGenerator _tileGenerator;
    [SerializeField] private int _initialRevealedRange;

    public readonly HashSet<(int x, int y)> RevealedTiles = new HashSet<(int x, int y)>();
    public readonly Dictionary<(int x, int y), BuildingType> Buildings = new Dictionary<(int x, int y), BuildingType>();

    public delegate void BuildingPlacedHandler(int x, int y, BuildingType buildingType);
    public event BuildingPlacedHandler BuildingPlaced;

    private Tilemap _tilemap;
    private ITileGrid<TileType> _tileGrid;

    void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
        _tileGrid = new HexGrid<TileType>(_tileGenerator);

        ExploreTile(0,0, _initialRevealedRange);
    }

    public TileType GetTileAt(int x, int y)
    {
        if(RevealedTiles.Contains((x, y)))
            return _tileGrid.GetTileAt(x, y);

        return null;
    }

    public void SetTileAt(int x, int y, BuildingType buildingType)
    {
        _tileGrid.SetTileAt(x, y, buildingType);
        DrawTile(x, y);
    }

    public HashSet<(int x, int y)> GetRevealedPositionsInRange(int x, int y, int range)
    {
        var positions = new HashSet<(int x, int y)>(_tileGrid.GetPositionsInRange(x, y, range));
        positions.IntersectWith(RevealedTiles);
        return positions;
    }

    public int GetAdjacentTilesOfType(int x, int y, TileType tileType)
    {
        return GetRevealedPositionsInRange(x, y, 1)
            .Select(pos => GetTileAt(pos.x, pos.y))
            .Where(tile => tile == tileType)
            .Count();
    }

    public (int x, int y) WorldToCell(Vector3 worldPoint)
    {
        //TODO: Change to work in 3d
        var cellPos = _tilemap.WorldToCell(worldPoint);
        return (cellPos.x, cellPos.y);
    }

    public void ExploreTile(int x, int y, int explorationRange = 1)
    {
        RevealTile(x, y);
        var positions = _tileGrid.GetPositionsInRange(x, y, explorationRange);
        foreach(var pos in positions)
        {
            RevealTile(pos.x, pos.y);
        }
    }

    public void PlaceBuilding(int x, int y, BuildingType buildingType)
    {
        var tile = GetTileAt(x, y);

        if(!RevealedTiles.Contains((x, y)))
            return;

        if(!tile.CanBeBuiltUpon)
            return;

        SetTileAt(x, y, buildingType);

        Buildings[(x, y)] = buildingType;

        OnBuildingPlaced(x, y, buildingType);
    }

    public void RevealTile(int x, int y)
    {
        DrawTile(x, y);
        RevealedTiles.Add((x, y));
    }

    void DrawTile(int x, int y)
    {
        var cellPosition = new Vector3Int(x, y, 0);
        _tilemap.SetTile(cellPosition, _tileGrid.GetTileAt(cellPosition.x, cellPosition.y).TileBase);
    }

    void OnBuildingPlaced(int x, int y, BuildingType buildingType)
    {
        var handler = BuildingPlaced;
        if (handler != null)
        {
            handler(x, y, buildingType);
        }
    }
}
