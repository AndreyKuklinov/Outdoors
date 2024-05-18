using UnityEngine;

public class AllCollector : ScoreCalculator
{
    [SerializeField] PointsCollector[] _pointsCollectors;

    private int _total;

    void Start()
    {
        foreach(var collector in _pointsCollectors)
            collector.PointsCollected += AddPointsToTotal;
    }

    public override int GetScore()
    {
        return _total;
    }

    void AddPointsToTotal(int pointsCount)
    {
        _total += pointsCount;
    }
}