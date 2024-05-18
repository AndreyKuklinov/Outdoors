using System;
using UnityEngine;

public abstract class PointsCollector : MonoBehaviour
{
    public event Action<int> PointsCollected;

    [SerializeField] protected GameBoard _gameBoard;

    protected void BuildingPlaced(int x, int y, BuildingType buildingType)
    {
        var points = CollectPoints(x, y, buildingType);
        PointsCollected?.Invoke(points);
    }

    protected abstract int CollectPoints(int x, int y, BuildingType buildingType);
}
