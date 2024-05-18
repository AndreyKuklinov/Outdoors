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
        _text.text = $"Монеты: {_coinManager.CoinsCount}";

        if(_coinManager.NextIncome > 0)
        {
            _text.text += $" (+{_coinManager.NextIncome})";
        }
    }
}
