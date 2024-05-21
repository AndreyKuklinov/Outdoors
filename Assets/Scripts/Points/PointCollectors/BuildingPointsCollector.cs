using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPointsCollector : PointsCollector
{
    protected override int GetTotalAfterBuild(int x, int y, BuildingType buildingType)
    {
        var pointsTotal = 0;

        foreach(var building in _gameBoard.Buildings)
            pointsTotal += building.Value.CalculateScore(building.Key.x, building.Key.y, _gameBoard);

        return pointsTotal;
    }
}
