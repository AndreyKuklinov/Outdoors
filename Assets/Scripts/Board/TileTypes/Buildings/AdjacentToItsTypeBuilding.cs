using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName="BuildingTypes/AdjacentToItsTypeBuilding")]
public class AdjacentToItsTypeBuilding : BuildingType
{
    public override HashSet<(int x, int y)> GetScoringPositionsAfterBuild(int x, int y, GameBoard gameBoard)
    {
        return gameBoard.GetRevealedPositionsInRange(x, y, 1)
            .Where(pos => gameBoard.GetTileAt(pos.x, pos.y) == this)
            .ToHashSet();
    }
}
