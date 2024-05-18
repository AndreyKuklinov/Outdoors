using TMPro;
using UnityEngine;

public class ProgressLabel : MonoBehaviour
{
    [SerializeField] private PointsProgressBar _progressBar;
    [SerializeField] private string _pointsName;

    private TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _text.text = $"{_pointsName}: {(int)(_progressBar.Progress * 100)}%";
    }
}
