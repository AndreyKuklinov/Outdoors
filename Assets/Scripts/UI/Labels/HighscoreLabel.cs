using TMPro;
using UnityEngine;

public class HighscoreLabel : MonoBehaviour
{
    [SerializeField] Highscore _scoreType;

    private TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();

        if(_scoreType == Highscore.Daily)
        {
            var currentDateHash = DailyChallengeLoader.CurrentDateHash;
            var savedDateHash = PlayerPrefs.GetInt(SaveData.DailyScoreDateHash.ToString());
            if(currentDateHash != savedDateHash)
            {
                PlayerPrefs.SetInt(Highscore.Daily.ToString(), 0);
            }
        }
    }

    void Update()
    {
        _text.text = $"Best score: {PlayerPrefs.GetInt(_scoreType.ToString())}";
    }
}
