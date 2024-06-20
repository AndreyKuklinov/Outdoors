using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName="BuildingTypes/OneHexAwayFromItsTypeBuilding")]
public class OneHexAwayFromItsTypeBuilding : BuildingType
{
    public override HashSet<(int x, int y)> GetScoringPositionsAfterBuild(int x, int y, GameBoard gameBoard)
    {
        var r1 = gameBoard.GetRevealedPositionsInRange(x, y, 1);
        var r2 = gameBoard.GetRevealedPositionsInRange(x, y, 2);

        r2.ExceptWith(r1);

        return r2.Where(pos => gameBoard.GetTileAt(pos.x, pos.y) == this && !(pos.x == x && pos.y == y)).ToHashSet();
    }
}
