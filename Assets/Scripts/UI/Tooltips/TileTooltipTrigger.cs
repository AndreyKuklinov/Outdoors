using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTooltipTrigger : MonoBehaviour
{
    [SerializeField] TooltipManager _tooltipManager;
    [SerializeField] TileMouseTrigger _tileMouseTrigger;
    [SerializeField] GameBoard _gameBoard;

    void Start()
    {
        _tileMouseTrigger.OnPointerEnterTile += PointerEnterTile;
        _tileMouseTrigger.OnPointerExitTile += PointerExitTile;
    }

    public void PointerEnterTile((int tileX, int tileY) tilePos)
    {
        var tile = _gameBoard.GetTileAt(tilePos.tileX, tilePos.tileY);

        if(tile == null)
            return;

        _tooltipManager.Show(tile.TileName, tile.TooltipText);
    }

    public void PointerExitTile((int tileX, int tileY) tilePos)
    {
        _tooltipManager.Hide();
    }
}
