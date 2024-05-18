using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName="BuildingTypes/AdjacentToItsTypeBuilding")]
public class AdjacentToItsTypeBuilding : BuildingType
{
    public override int CalculateScore(int x, int y, GameBoard gameBoard)
    {
        return gameBoard.GetRevealedPositionsInRange(x, y, 1)
            .Where(pos => gameBoard.GetTileAt(pos.x, pos.y) == this)
            .Count();
    }
}
