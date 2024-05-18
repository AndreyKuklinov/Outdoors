using TMPro;
using UnityEngine;

public class LevelLabel : MonoBehaviour
{
    [SerializeField] private StageManager _stageManager;

    private TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _text.text = $"Уровень: {_stageManager.CurrentStage}";
    }
}
