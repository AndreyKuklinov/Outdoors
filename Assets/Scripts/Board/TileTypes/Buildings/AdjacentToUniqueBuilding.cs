using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="BuildingTypes/AdjacentToUniqueBuilding")]
public class AdjacentToUniqueBuilding : BuildingType
{
    public override int CalculateScore(int x, int y, GameBoard gameBoard)
    {
        var uniqueBuildings = new HashSet<TileType>();
        var nonUniqueBuildings = new HashSet<TileType>();

        foreach(var pos in gameBoard.GetRevealedPositionsInRange(x, y ,1))
        {
            if(!gameBoard.Buildings.ContainsKey((pos.x, pos.y)))
                continue;

            var buildingType = gameBoard.GetTileAt(pos.x, pos.y);

            if(buildingType == this)
                continue;

            if(!uniqueBuildings.Contains(buildingType) && !nonUniqueBuildings.Contains(buildingType))
            {
                uniqueBuildings.Add(buildingType);
            }

            else if(uniqueBuildings.Contains(buildingType))
            {
                uniqueBuildings.Remove(buildingType);
                nonUniqueBuildings.Add(buildingType);
            }
        }

        return uniqueBuildings.Count;
    }
}
