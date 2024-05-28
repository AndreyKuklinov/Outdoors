using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMouseTrigger : MonoBehaviour
{
    [SerializeField] MouseMovedDetector _mouseMovedDetector;
    [SerializeField] GameBoard _gameBoard;

    public event Action<(int tileX, int tileY)> OnPointerEnterTile;
    public event Action<(int tileX, int tileY)> OnPointerExitTile;

    private (int tileX, int tileY) _pointerOnTile;
    private bool _isPointerOnAnyTile = false;

    void Start()
    {
        _mouseMovedDetector.MouseMoved += CheckForTileEvent;
    }

    void CheckForTileEvent()
    {
        var worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var cellPos = _gameBoard.WorldToCell(worldPoint);

        if(!_isPointerOnAnyTile)
        {
            _isPointerOnAnyTile = true;
            _pointerOnTile = cellPos;
            OnPointerEnterTile?.Invoke(cellPos);
            return;
        }

        if(_pointerOnTile != cellPos)
        {
            OnPointerExitTile?.Invoke(_pointerOnTile);
            OnPointerEnterTile?.Invoke(cellPos);
            _pointerOnTile = cellPos;
        }
    }
}
