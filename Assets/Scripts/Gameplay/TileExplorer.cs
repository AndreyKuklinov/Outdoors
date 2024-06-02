using System;
using UnityEngine;

public class TileExplorer : MonoBehaviour
{
    [SerializeField] private TileRaycaster _raycaster;
    [SerializeField] private GameBoard _gameBoard;
    [SerializeField] private PurchaseManager _coinManager;
    [SerializeField] private bool _canExploreInLand;

    public event Action<TileType> TileExplored;

    void ExploreTile(int x, int y)
    {
        if(!CanExplore(x, y))
            return;

        var tileType = _gameBoard.GetTileAt(x, y);

        _gameBoard.ExploreTile(x, y);
        _coinManager.PayForPurchase();
        OnTileExplored(tileType);
    }

    void Update()
    {
        HandleClick();
    }

    bool CanExplore(int x, int y)
    {
        return _coinManager.CanPurchase
            && _gameBoard.RevealedTiles.Contains((x, y))
            && (_canExploreInLand || _gameBoard.HasUnrevealedNeighbours(x, y));
    }

    void HandleClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var tilePos = _raycaster.GetTilePosition(Input.mousePosition);
            ExploreTile(tilePos.x, tilePos.y);
        }
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
