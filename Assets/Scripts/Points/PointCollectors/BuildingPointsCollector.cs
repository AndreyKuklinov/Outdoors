using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPointsCollector : PointsCollector
{
    public int PointsTotal { get; private set; }

    void Start()
    {
        _gameBoard.BuildingPlaced += BuildingPlaced;
    }

    protected override int CollectPoints(int x, int y, BuildingType buildingType)
    {
        var newTotal = GetNewPointsTotal();
        var oldTotal = PointsTotal;
        PointsTotal = newTotal;

        return newTotal - oldTotal;
    }

    protected int GetNewPointsTotal()
    {
        var pointsTotal = 0;

        foreach(var building in _gameBoard.Buildings)
            pointsTotal += building.Value.CalculateScore(building.Key.x, building.Key.y, _gameBoard);

        return pointsTotal;
    }
}
