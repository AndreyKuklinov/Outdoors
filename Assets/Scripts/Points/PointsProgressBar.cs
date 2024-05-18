using System;
using UnityEngine;

public class PointsProgressBar : MonoBehaviour
{
    [SerializeField] private PointsCollector _pointsCollector;
    [SerializeField] private float _threshold;
    [SerializeField] private float _pointsTotal;

    public event Action PointsTotalChanged;

    public float Progress
        => Mathf.Min(_pointsTotal / _threshold, 1);

    public bool IsFull
        => _pointsTotal >= _threshold;

    void Start()
    {
        _pointsCollector.PointsCollected += CollectPoints;
    }

    public void SetThreshold(float threshold)
    {
        _threshold = threshold;
    }

    public void RemoveOverflow()
    {
        if(!IsFull)
            return;

        _pointsTotal -= _threshold;
    }

    void CollectPoints(int pointsCount)
    {
        _pointsTotal += pointsCount;
        if(_pointsTotal < 0)
            _pointsTotal = 0;
        PointsTotalChanged?.Invoke();
    }
}
