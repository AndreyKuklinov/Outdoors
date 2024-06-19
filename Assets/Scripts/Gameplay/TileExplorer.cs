using System;
using UnityEngine;

public class TileExplorer : MonoBehaviour
{
    [SerializeField] private TileRaycaster _raycaster;
    [SerializeField] private GameBoard _gameBoard;
    [SerializeField] private PurchaseManager _coinManager;
    [SerializeField] private bool _canExploreInLand;

    public event Action<TileType> TileExplored;
    public event Action<(int x, int y)> CantExploreNoMoney;
    public event Action<(int x, int y)> CantExploreInland;

    public void ExploreTile(int x, int y)
    {
        if(!CanExplore(x, y))
            return;

        var tileType = _gameBoard.GetTileAt(x, y);

        _gameBoard.ExploreTile(x, y);
        _coinManager.PayForPurchase();
        OnTileExplored(tileType);
    }

    bool CanExplore(int x, int y)
    {
        if(!_gameBoard.RevealedTiles.Contains((x, y)))
        {
            return false;
        }

        if(!_coinManager.CanPurchase)
        {
            CantExploreNoMoney?.Invoke((x, y));
            return false;
        }

        if(!_canExploreInLand && !_gameBoard.HasUnrevealedNeighbours(x,y))
        {
            CantExploreInland?.Invoke((x, y));
            return false;
        }

        return true;
    }

    void OnTileExplored(TileType tileType)
    {
        var handler = TileExplored;
        if (handler != null)
        {
            handler(tileType);
        }
    }
}
