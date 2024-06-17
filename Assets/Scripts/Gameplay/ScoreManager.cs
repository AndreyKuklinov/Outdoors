using System;
using UnityEngine;

public enum Highscores
{
    Daily,
    Overall
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
        _bestDaily = PlayerPrefs.GetInt(nameof(Highscores.Daily));
        _bestOverall = PlayerPrefs.GetInt(nameof(Highscores.Overall));
        Debug.Log(_bestOverall);
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
            PlayerPrefs.SetInt(nameof(Highscores.Daily), Score);

            if(_bestDaily != 0)
                HighscoreAchieved?.Invoke();

            _bestDaily = Score;
        }
    }

    void UpdateOverallScore()
    {
        if(Score > _bestOverall)
        {
            PlayerPrefs.SetInt(nameof(Highscores.Overall), Score);

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