using System;
using UnityEngine;

public class StagedGameManager : MonoBehaviour
{
    [SerializeField] private PointsCollector _incomeCollector;
    [SerializeField] private PointsProgressBar _progressBar;
    [SerializeField] private CoinManager _coinManager;
    [SerializeField] private StagedValueGenerator<int> _thresholdGenerator;

    [field: SerializeField] public int CurrentStage { get; private set; }

    void Start()
    {
        _progressBar.PointsTotalChanged += TryMoveToNextStage;
        _incomeCollector.PointsCollected += _coinManager.IncreaseIncome;
        MoveToStage(CurrentStage);
    }

    void TryMoveToNextStage()
    {
        while(_progressBar.IsFull)
        {
            _progressBar.RemoveOverflow();
            MoveToStage(CurrentStage+1);
        }
    }

    void MoveToStage(int stageNumber)
    {
        CurrentStage = stageNumber;

        var newThreshold = _thresholdGenerator.GetValueAtStage(stageNumber);
        _progressBar.SetThreshold(newThreshold);

        _coinManager.ReceiveIncome();
    }
}
