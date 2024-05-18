using TMPro;
using UnityEngine;

public class CoinsLabel : MonoBehaviour
{
    [SerializeField] private CoinManager _coinManager;

    private TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _text.text = $"Монеты: {(int)(_coinManager.CoinsCount / (float)_coinManager.InitialCoinsCount * 100)}%";

        if(_coinManager.NextIncome > 0)
        {
            _text.text += $" (+{(int)(_coinManager.NextIncome / (float)_coinManager.InitialCoinsCount * 100)}%)";
        }
    }
}
