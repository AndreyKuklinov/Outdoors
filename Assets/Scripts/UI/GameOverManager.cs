using UnityEngine;
using UnityEngine.Rendering;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] PurchaseManager _purchaseManager;
    [SerializeField] GameObject _restartButton;
    [SerializeField] BuildingHand _buildingHand;
    

    void Update()
    {
        if(!_purchaseManager.CanPurchase && _buildingHand.BuildingsCount == 0)
            GameOver();
    }

    void GameOver()
    {
        _restartButton.SetActive(true);
    }
}