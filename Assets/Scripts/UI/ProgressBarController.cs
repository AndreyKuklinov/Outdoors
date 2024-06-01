using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _image;
    [SerializeField] private PurchaseManager _purchaseManager;

    void Update()
    {
        var amount = (float)_purchaseManager.CoinsCount / _purchaseManager.InitialCoinsCount;
        _image.fillAmount = amount;
    }
}
