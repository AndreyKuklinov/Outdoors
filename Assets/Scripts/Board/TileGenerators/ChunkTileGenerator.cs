using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName="TileGenerators/ChunkTileGenerator")]
public class ChunkTileGenerator : ScriptableTileGenerator
{
    [SerializeField] private TileType[] _possibleTiles;
    [SerializeField] private int _chunkSize;

    private readonly Dictionary<(int x, int y), TileType[,]> _chunks = new Dictionary<(int x, int y), TileType[,]>();
    private System.Random _random = new System.Random();
    private int? _seed = null;


    private int MaxCountOfTileTypeInChunk => _chunkSize * _chunkSize / _possibleTiles.Length;
//    private int ChunkSize => _chunkRadius * 2 - 1;

    public override void SetSeed(int seed)
    {
        _random = new System.Random(seed);
        _seed = seed;
    }

    public override TileType GenerateAt(int x, int y)
    {
        if(_chunkSize % 2 == 0)
            throw new NotImplementedException("Tile generation for even chunk sizes is not implemented");

        (int chunkX, int chunkY) = GetChunkCoordinates(x, y);
        (int tileX, int tileY) = GetTileCoordsRelativeToChunk(x, y);
        var chunk = GetChunk(chunkX, chunkY);
        return chunk[tileX, tileY];
    }

    (int chunkX, int chunkY) GetChunkCoordinates(int tileX, int tileY)
    {
        return (tileX / (_chunkSize / 2 + 1), tileY / (_chunkSize / 2 + 1));
    }

    (int x, int y) GetTileCoordsRelativeToChunk(int x, int y)
    {
        (int resX, int resY) = (GetRelativeCoord(x), GetRelativeCoord(y));

//        (int chunkX, int chunkY) = GetChunkCoordinates(x, y);
//        Debug.Log($"[{x}, {y}] => [{resX}, {resY}] (in {chunkX}, {chunkY})");

        return (resX, resY);
    }

    int GetRelativeCoord(int coord)
    {
        var res = (coord + _chunkSize / 2) % _chunkSize;
        if(res < 0)
            res += _chunkSize;

        return res;
    }

    TileType[,] GetChunk(int chunkX, int chunkY)
    {
        if(!_chunks.ContainsKey((chunkX, chunkY)))
            GenerateChunk(chunkX, chunkY);

        return _chunks[(chunkX, chunkY)];
    }

    void GenerateChunk(int chunkX, int chunkY)
    {
//        Debug.Log($"Welcome to a new chunk! {chunkX} {chunkY}");

        var typeCounts = new Dictionary<TileType, int>();
        var chunkRandom = GetRandomForChunk(chunkX, chunkY);
        var availableTileTypes = new HashSet<TileType>(_possibleTiles);

        foreach(var tile in _possibleTiles)
            typeCounts.Add(tile, 0);

        _chunks[(chunkX, chunkY)] = new TileType[_chunkSize, _chunkSize];
        for(var tileX = 0; tileX < _chunkSize; tileX++)
        {
            for(var tileY = 0; tileY < _chunkSize; tileY++)
            {
                TileType type;
                if(availableTileTypes.Count > 0)
                    type = availableTileTypes.OrderBy(x => chunkRandom.Next()).First();
                else
                {
//                    Debug.Log("Improvising!");
                    type = _possibleTiles[chunkRandom.Next(0, _possibleTiles.Length)];
                }
                _chunks[(chunkX, chunkY)][tileX, tileY] = type;

                typeCounts[type]++;
                if(typeCounts[type] == MaxCountOfTileTypeInChunk)
                    availableTileTypes.Remove(type);
            }
        }

        // TESTING
//        foreach(var pair in typeCounts)
//        {
//            Debug.Log($"{pair.Key}: {pair.Value}");
//        }
    }

    System.Random GetRandomForChunk(int chunkX, int chunkY)
    {
        if(_seed == null)
            return new System.Random();

        var hash = Tuple.Create(_seed, chunkX, chunkY).GetHashCode();
        return new System.Random(hash);
    }
}
