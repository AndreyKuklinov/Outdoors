using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPointsCollector : PointsCollector
{
    private Dictionary<(int x, int y), int> _prevPointsAtPos = new Dictionary<(int x, int y), int>();

    public event Action<(int x, int y, int sum)> BuildingCollectedPoints;

    protected override int GetTotalAfterBuild(int x, int y, BuildingType buildingType)
    {
        var pointsTotal = 0;

        foreach(var building in _gameBoard.Buildings)
        {
            var b_x = building.Key.x;
            var b_y = building.Key.y;
            var buildingScore = building.Value.CalculateScore(building.Key.x, building.Key.y, _gameBoard);

            if(_prevPointsAtPos.ContainsKey((b_x, b_y)))
            {
                var diff = buildingScore - _prevPointsAtPos[(b_x, b_y)];
                if(diff != 0)
                    BuildingCollectedPoints?.Invoke((b_x, b_y, diff));
                _prevPointsAtPos[(b_x,b_y)] = buildingScore;
            }

            else
            {
                if(buildingScore != 0)
                    BuildingCollectedPoints?.Invoke((b_x, b_y, buildingScore));
                _prevPointsAtPos.Add((b_x, b_y), buildingScore);
            }

            pointsTotal += buildingScore;
        }

        return pointsTotal;
    }
}
