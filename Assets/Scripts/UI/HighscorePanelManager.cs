using UnityEngine;

public class HighscorePanelManager : MonoBehaviour
{
    [SerializeField] GameObject _highscorePanel;
    [SerializeField] ScoreManager _scoreManager;

    void Start()
    {
        _scoreManager.HighscoreAchieved += HighscoreAchieved;
    }

    void HighscoreAchieved()
    {
        _highscorePanel.SetActive(true);
    }
}