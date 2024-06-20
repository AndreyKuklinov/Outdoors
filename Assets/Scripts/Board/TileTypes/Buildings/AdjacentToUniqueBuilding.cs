using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName="BuildingTypes/AdjacentToUniqueBuilding")]
public class AdjacentToUniqueBuilding : BuildingType
{
    public override HashSet<(int x, int y)> GetScoringPositionsAfterBuild(int x, int y, GameBoard gameBoard)
    {
        var uniqueBuildings = new Dictionary<TileType, (int x, int y)>();
        var nonUniqueBuildings = new HashSet<TileType>();

        foreach(var pos in gameBoard.GetRevealedPositionsInRange(x, y ,1))
        {
            if(!gameBoard.Buildings.ContainsKey((pos.x, pos.y)))
                continue;

            var buildingType = gameBoard.GetTileAt(pos.x, pos.y);

            if(buildingType == this)
                continue;

            if(!uniqueBuildings.ContainsKey(buildingType) && !nonUniqueBuildings.Contains(buildingType))
            {
                uniqueBuildings.Add(buildingType, pos);
            }

            else if(uniqueBuildings.ContainsKey(buildingType))
            {
                uniqueBuildings.Remove(buildingType);
                nonUniqueBuildings.Add(buildingType);
            }
        }

        return uniqueBuildings.Values.ToHashSet();
    }
}
