using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPointsCollector : PointsCollector
{
    protected override int GetTotalAfterBuild(int x, int y, BuildingType buildingType)
    {
        return PointsTotal + _gameBoard.GetAdjacentTilesOfType(x, y, buildingType.AdjacencyBonusTerrainType);
    }
}
