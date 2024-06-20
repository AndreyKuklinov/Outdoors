using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingType : TileType
{
    [field: SerializeField] public Sprite HandElementSprite { get; private set; }
    [field: SerializeField] public TerrainType AdjacencyBonusTerrainType { get; private set; }
    public override bool CanBeBuiltUpon => false;
    public override BuildingType ExplorationBuildingType => this;

    public int CalculateScore(int x, int y, GameBoard gameBoard)
    {
        return GetScoringPositionsAfterBuild(x, y, gameBoard).Count;
    }

    public abstract HashSet<(int x, int y)> GetScoringPositionsAfterBuild(int x, int y, GameBoard gameBoard);
}
