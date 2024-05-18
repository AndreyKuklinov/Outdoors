using System;
using UnityEngine;

public class SimpleGameManager : MonoBehaviour
{
    [SerializeField] private CoinManager _coinManager;
    [SerializeField] private PointsCollector _incomeCollector;
    [SerializeField] private BuildingPointsCollector _scoreCollector;

    public int Score
        => _scoreCollector.PointsTotal;

    void Start()
    {
        _incomeCollector.PointsCollected += IncomeCollected;
    }

    void IncomeCollected(int collectedIncome)
    {
        _coinManager.IncreaseIncome(collectedIncome);
        _coinManager.ReceiveIncome();
    }
}
