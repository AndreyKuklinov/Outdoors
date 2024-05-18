using UnityEngine;

public class SimpleScoreCalculator : ScoreCalculator
{
    [field: SerializeField] SimpleGameManager _gameManager;

    public override int GetScore()
        => _gameManager.Score;
}