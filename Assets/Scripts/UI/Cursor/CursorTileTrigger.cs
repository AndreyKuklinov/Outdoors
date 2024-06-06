using UnityEngine;

public class CursorTileTrigger : MonoBehaviour
{
    [SerializeField] TileMouseTrigger _tileMouseTrigger;
    [SerializeField] GameBoard _gameBoard;
    private bool _isCursorOverTile;

    void Start()
    {
        _tileMouseTrigger.OnPointerEnterTile += OnPointerEnterTile;
        _tileMouseTrigger.OnPointerExitTile += OnPointerExitTile;
    }

    void OnPointerEnterTile((int x, int y) pos)
    {
        if(!IsTileRevealed(pos))
            return;

        _isCursorOverTile = true;
        CursorManager.Singleton.SetCursorOverClicklableUI(true);
    }

    void OnPointerExitTile((int x, int y) pos)
    {
        if(!_isCursorOverTile)
            return;

        _isCursorOverTile = false;
        CursorManager.Singleton.SetCursorOverClicklableUI(false);
    }

    bool IsTileRevealed((int x, int y) pos)
        => _gameBoard.RevealedTiles.Contains(pos);
}