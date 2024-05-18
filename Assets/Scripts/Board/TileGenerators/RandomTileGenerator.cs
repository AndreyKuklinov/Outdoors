using UnityEngine;
using System;

[CreateAssetMenu(menuName="TileGenerators/RandomGenerator")]
public class RandomTileGenerator : ScriptableTileGenerator
{
    [SerializeField] private TileType[] _possibleTiles;

    public override void SetSeed(int seed)
    {
        throw new NotImplementedException();
    }

    public override TileType GenerateAt(int x, int y) {
        var rng = UnityEngine.Random.Range(0, _possibleTiles.Length);
        return _possibleTiles[rng];
    }

}
