using System;
using UnityEngine;

public class TileExplorer : MonoBehaviour
{
    [SerializeField] private GameBoard _gameBoard;
    [SerializeField] private CoinManager _coinManager;

    public event Action<TileType> TileExplored;

    void ExploreTile(int x, int y)
    {
        if(!_coinManager.CanPurchase || !_gameBoard.RevealedTiles.Contains((x, y)))
            return;

        _gameBoard.ExploreTile(x, y);
        _coinManager.PayForPurchase();
        OnTileExplored(_gameBoard.GetTileAt(x, y));
    }

    void Update()
    {
        HandleClick();
    }

    void HandleClick()
    {
        var worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var cellPos = _gameBoard.WorldToCell(worldPoint);

        if (Input.GetMouseButtonDown(1))
        {
            ExploreTile(cellPos.x, cellPos.y);
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
