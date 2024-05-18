using System.Collections.Generic;
using System;

public class HexGrid<T> : ITileGrid<T>
{
    public static readonly (int x, int y)[] OddOffsets = new (int x, int y)[]
    {
        (0, 1),
        (1, 1),
        (1, 0),
        (1, -1),
        (0, -1),
        (-1, 0)
    };

    public static readonly (int x, int y)[] EvenOffsets = new (int x, int y)[]
    {
        (-1, 1),
        (0, 1),
        (1, 0),
        (0, -1),
        (-1, -1),
        (-1, 0)
    };

    private Dictionary<(int x, int y), T> _tiles;
    private ITileGenerator<T> _tileGenerator;

    public HexGrid(ITileGenerator<T> tileGenerator)
    {
        _tiles = new Dictionary<(int x, int y), T>();
        _tileGenerator = tileGenerator;
    }

    public T GetTileAt(int x, int y)
    {
        if(_tiles.ContainsKey((x, y)))
            return _tiles[(x,y)];

        var tile = _tileGenerator.GenerateAt(x, y);
        SetTileAt(x, y, tile);
        return tile;
    }

    public void SetTileAt(int x, int y, T tile) {
        if(_tiles.ContainsKey((x, y)))
            _tiles[(x,y)] = tile;

        else
            _tiles.Add((x,y), tile);
    }

    public HashSet<(int x, int y)> GetPositionsInRange(int x, int y, int range)
    {
        if(range == 0)
            return new HashSet<(int x, int y)>(new (int x, int y)[] {(x, y)});

        var positions = new HashSet<(int x, int y)>();

        var adjacencyOffsets = y % 2 == 0 ? EvenOffsets : OddOffsets;

        foreach(var offset in adjacencyOffsets)
        {
            positions.UnionWith(GetPositionsInRange(x + offset.x, y + offset.y, range-1));
        }

        return positions;
    }
}
