using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    [field: SerializeField] StageManager _stageManager;
    [field: SerializeField] PointsProgressBar _progressBar;

    public int Score
        => _stageManager.CurrentStage * 100 + (int)(_progressBar.Progress * 100);
}