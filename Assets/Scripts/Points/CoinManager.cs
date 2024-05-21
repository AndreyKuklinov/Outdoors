using System;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private int _purchaseCost;

    [field: SerializeField] public int CoinsCount { get; private set; }

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
}
