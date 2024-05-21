using System;
using UnityEngine;

public abstract class PointsCollector : MonoBehaviour
{
    [field: SerializeField] public int PointsTotal { get; private set; }

    [SerializeField] protected GameBoard _gameBoard;

    public event Action PointsCollected;

    void Start()
    {
        _gameBoard.BuildingPlaced += BuildingPlaced;
    }

    protected void BuildingPlaced(int x, int y, BuildingType buildingType)
    {
        PointsTotal = GetTotalAfterBuild(x, y, buildingType);
        PointsCollected?.Invoke();
    }

    protected abstract int GetTotalAfterBuild(int x, int y, BuildingType buildingType);
}
