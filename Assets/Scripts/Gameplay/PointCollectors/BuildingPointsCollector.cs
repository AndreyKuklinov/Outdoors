using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPointsCollector : PointsCollector
{
    private Dictionary<(int x, int y), int> _prevPointsAtPos = new Dictionary<(int x, int y), int>();

    protected override int GetTotalAfterBuild(int x, int y, BuildingType buildingType)
    {
        return PointsTotal + buildingType.CalculateScore(x, y, _gameBoard);
    }
}
