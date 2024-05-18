using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPointsCollector : PointsCollector
{
    void Awake()
    {
        _gameBoard.BuildingPlaced += BuildingPlaced;
    }

    protected override int CollectPoints(int x, int y, BuildingType buildingType)
    {
        return _gameBoard.GetAdjacentTilesOfType(x, y, buildingType.AdjacencyBonusTerrainType);
    }
}
