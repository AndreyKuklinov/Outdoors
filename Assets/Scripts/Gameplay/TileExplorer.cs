using System;
using UnityEngine;

public class TileExplorer : MonoBehaviour
{
    [SerializeField] private GameBoard _gameBoard;
    [SerializeField] private CoinManager _coinManager;
    [SerializeField] private bool _allowToExploreInlandTiles;

    public event Action<TileType> TileExplored;

    void ExploreTile(int x, int y)
    {
        if(!_allowToExploreInlandTiles && !_gameBoard.HasUnrevealedNeighbours(x, y))
            return;

        if(!_coinManager.CanPurchase || !_gameBoard.RevealedTiles.Contains((x, y)))
            return;

        var tileType = _gameBoard.GetTileAt(x, y);

        if(tileType.ExplorationBuildingType == null)
            return;

        _gameBoard.ExploreTile(x, y);
        _coinManager.PayForPurchase();
        OnTileExplored(tileType);
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
