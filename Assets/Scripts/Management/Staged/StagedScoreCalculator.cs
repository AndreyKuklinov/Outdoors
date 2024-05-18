using UnityEngine;

public class StagedScoreCalculator : ScoreCalculator
{
    [field: SerializeField] StagedGameManager _stageManager;
    [field: SerializeField] PointsProgressBar _progressBar;

    public override int GetScore()
        => _stageManager.CurrentStage * 100 + (int)(_progressBar.Progress * 100);
}