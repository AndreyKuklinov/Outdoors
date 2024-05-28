using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTooltipTrigger : MonoBehaviour
{
    [SerializeField] GameBoard _gameBoard;

    private Vector3 _prevMousePos;
    private (int x, int y) _lastCellPos;
    private bool _isPointerOnTile;

    void Update()
    {
        if(CheckIfMouseMoved())
            MouseMoved();        
    }

    void MouseMoved()
    {
        var worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var cellPos = _gameBoard.WorldToCell(worldPoint);
        var tile = _gameBoard.GetTileAt(cellPos.x, cellPos.y);

        if(tile == null)
        {
            if(_isPointerOnTile)
                TooltipManager.Manager.Hide();

            _isPointerOnTile = false;
            return;
        }

        if(cellPos != _lastCellPos && _isPointerOnTile) 
            TooltipManager.Manager.Hide();

        _isPointerOnTile = true;
        _lastCellPos = cellPos;
        TooltipManager.Manager.Show(tile.TileName, tile.TooltipText);
    }

    bool CheckIfMouseMoved()
    {
        var mousePos = Input.mousePosition;

        if(mousePos == _prevMousePos)
            return false;

        _prevMousePos = mousePos;
        return true;
    }
}
