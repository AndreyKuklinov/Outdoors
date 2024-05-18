using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName="BuildingTypes/AdjacentToNonUniqueBuilding")]
public class AdjacentToNonUniqueBuilding : BuildingType
{
    public override int CalculateScore(int x, int y, GameBoard gameBoard)
    {
        var buildingCounts = new Dictionary<TileType, int>();

        foreach(var pos in gameBoard.GetRevealedPositionsInRange(x, y ,1))
        {
            if(!gameBoard.Buildings.ContainsKey((pos.x, pos.y)))
                continue;

            var buildingType = gameBoard.GetTileAt(pos.x, pos.y);

            if(buildingType == this)
                continue;

            IncreaseCount(buildingCounts, buildingType);
        }

        return buildingCounts
            .Where(pair => pair.Value >= 2)
            .Sum(pair => pair.Value);
    }

    void IncreaseCount(Dictionary<TileType, int> dict, TileType type)
    {
        if(!dict.ContainsKey(type))
            dict[type] = 0;

        dict[type]++;
    }
}
