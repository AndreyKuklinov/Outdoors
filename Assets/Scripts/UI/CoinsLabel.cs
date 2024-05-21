using TMPro;
using UnityEngine;

public class CoinsLabel : MonoBehaviour
{
    [SerializeField] private PurchaseManager _coinManager;

    private TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
//        _text.text = $"Монеты: {(int)(_coinManager.CoinsCount / (float)_coinManager.InitialCoinsCount * 100)}%";
        _text.text = $"Монеты: {(int)(_coinManager.CoinsCount / (float)_coinManager.PurchaseCost)}";
    }
}
