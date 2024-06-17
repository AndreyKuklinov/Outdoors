using System;
using UnityEngine;

public enum Highscore
{
    Daily,
    Overall
}

public enum SaveData
{
    DailyScoreDateHash
}

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TerrainPointsCollector _terrainCollector;
    [SerializeField] private BuildingPointsCollector _buildingsCollector;

    private int _bestDaily;
    private int _bestOverall;

    public int Score
        => _terrainCollector.PointsTotal * _buildingsCollector.PointsTotal;

    public event Action HighscoreAchieved;

    void Start()
    {
        _bestDaily = PlayerPrefs.GetInt(nameof(Highscore.Daily));
        _bestOverall = PlayerPrefs.GetInt(nameof(Highscore.Overall));
    }

    void Update()
    {
        UpdateDailyScore();
        UpdateOverallScore();
    }

    void UpdateDailyScore()
    {
        if(!SceneNavigator.IsDaily)
            return;

        if(Score > _bestDaily)
        {
            PlayerPrefs.SetInt(Highscore.Daily.ToString(), Score);
            PlayerPrefs.SetInt(SaveData.DailyScoreDateHash.ToString(), DailyChallengeLoader.CurrentDateHash);

            if(_bestDaily != 0)
                HighscoreAchieved?.Invoke();

            _bestDaily = Score;
        }
    }

    void UpdateOverallScore()
    {
        if(Score > _bestOverall)
        {
            PlayerPrefs.SetInt(Highscore.Overall.ToString(), Score);

            if(_bestOverall != 0)
                HighscoreAchieved?.Invoke();

            _bestOverall = Score;
        }
    }

    [ContextMenu("Reset scores")]
    void ResetScores()
    {
        PlayerPrefs.DeleteAll();
    }
}