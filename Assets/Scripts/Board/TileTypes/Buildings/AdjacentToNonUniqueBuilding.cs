using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName="BuildingTypes/AdjacentToNonUniqueBuilding")]
public class AdjacentToNonUniqueBuilding : BuildingType
{
    public override HashSet<(int x, int y)> GetScoringPositionsAfterBuild(int x, int y, GameBoard gameBoard)
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

        var qualifiedTypes = buildingCounts.Where(pair => pair.Value >= 2).Select(pair => pair.Key).ToHashSet();
        return gameBoard.GetRevealedPositionsInRange(x, y, 1).Where(pos => qualifiedTypes.Contains(gameBoard.GetTileAt(pos.x, pos.y))).ToHashSet();
    }

    void IncreaseCount(Dictionary<TileType, int> dict, TileType type)
    {
        if(!dict.ContainsKey(type))
            dict[type] = 0;

        dict[type]++;
    }
}
