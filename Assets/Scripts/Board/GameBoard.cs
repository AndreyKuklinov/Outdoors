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

    public Tilemap Tilemap { get; private set; }
    private ITileGrid<TileType> _tileGrid;

    void Awake()
    {
        Tilemap = GetComponent<Tilemap>();
        _tileGenerator.SetSeed(SceneNavigator.GameSeed);
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
        var cellPos = Tilemap.WorldToCell(worldPoint);
        return (cellPos.x, cellPos.y);
    }

    public bool HasUnrevealedNeighbours(int x, int y)
    {
        return _tileGrid.GetPositionsInRange(x, y, 1)
            .Any(pos => !RevealedTiles.Contains(pos));
    }

    public void ExploreTile(int x, int y, int explorationRange = 1)
    {
        RevealTile(x, y);
        var positions = _tileGrid.GetPositionsInRange(x, y, explorationRange);
        foreach(var pos in positions)
        {
            RevealTile(pos.x, pos.y);
        }

        var foggyPositions = _tileGrid.GetPositionsInRange(x, y, explorationRange+1);
        foreach(var pos in foggyPositions)
        {
            DrawFoggyTile(pos.x, pos.y);
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
        if(RevealedTiles.Contains((x, y)))
            return;

        var cellPosition = new Vector3Int(x, y, 0);
        Tilemap.SetTile(cellPosition, _tileGrid.GetTileAt(cellPosition.x, cellPosition.y).TileBase);
    }

    void DrawFoggyTile(int x, int y)
    {
        if(RevealedTiles.Contains((x, y)))
            return;

        var cellPosition = new Vector3Int(x, y, 0);
        var tileType = _tileGrid.GetTileAt(cellPosition.x, cellPosition.y) as TerrainType;

        Tilemap.SetTile(cellPosition, tileType.FoggyVersion);
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
