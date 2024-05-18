using System;
using UnityEngine;

public abstract class ScriptableTileGenerator : ScriptableObject, ITileGenerator<TileType>
{
    public abstract void SetSeed(int seed);
    public abstract TileType GenerateAt(int x, int y);
}
