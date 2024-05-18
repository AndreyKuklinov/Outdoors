using System;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private int _purchaseCost;

    [field: SerializeField] public int CoinsCount { get; private set; }
    [field: SerializeField] public int NextIncome { get; private set; }

    public int InitialCoinsCount { get; private set; }

    public bool CanPurchase
        => CoinsCount >= _purchaseCost;

    void Start()
    {
        InitialCoinsCount = CoinsCount;
    }

    public void PayForPurchase()
    {
        if(!CanPurchase)
            throw new Exception("A purchase was made with not enough coins");

        CoinsCount -= _purchaseCost;
    }

    public void ReceiveIncome()
    {
        CoinsCount += NextIncome;
        NextIncome = 0;
    }

    public void IncreaseIncome(int increase)
    {
        NextIncome += increase;
    }
}
