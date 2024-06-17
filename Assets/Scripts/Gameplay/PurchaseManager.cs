using System;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
    [field: SerializeField] public int PurchaseCost { get; private set; }
    [field: SerializeField] public int CoinsCount { get; private set; }

    public int InitialCoinsCount { get; private set; }

    public event Action OutOfMoney;

    public bool CanPurchase
        => CoinsCount >= PurchaseCost;

    void Start()
    {
        InitialCoinsCount = CoinsCount;
    }

    public void PayForPurchase()
    {
        if(!CanPurchase)
            throw new Exception("A purchase was made with not enough coins");

        CoinsCount -= PurchaseCost;

        if(!CanPurchase)
            OutOfMoney?.Invoke();
    }
}
