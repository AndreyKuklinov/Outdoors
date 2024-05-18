using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="BuildingTypes/AdjacentToTerrainTypeBuilding")]
public class AdjacentToTerrainTypeBuilding : BuildingType
{
    [field: SerializeField] public TileType ScoringTerrainType { get; private set; }

    public override int CalculateScore(int x, int y, GameBoard gameBoard)
    {
        var score = 0;
        foreach(var pos in gameBoard.GetRevealedPositionsInRange(x, y, 1))
        {
            if(!gameBoard.Buildings.ContainsKey((pos.x, pos.y)) || gameBoard.Buildings[(pos.x, pos.y)] == this)
                continue;

            foreach(var p in gameBoard.GetRevealedPositionsInRange(pos.x, pos.y, 1))
            {
                if(gameBoard.GetTileAt(p.x, p.y) == ScoringTerrainType)
                {
                    score++;
                    break;
                }
            }
        }

        return score;
    }
}
