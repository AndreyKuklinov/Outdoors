using TMPro;
using UnityEngine;

public class LevelLabel : MonoBehaviour
{
    [SerializeField] private StagedGameManager _stageManager;

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
