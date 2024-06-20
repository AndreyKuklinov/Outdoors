using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName="BuildingTypes/InLineWithItsTypeBuilding")]
public class InLineWithItsTypeBuilding : BuildingType
{
    public override HashSet<(int x, int y)> GetScoringPositionsAfterBuild(int x, int y, GameBoard gameBoard)
    {
        var result = new HashSet<(int x, int y)>();
        for(var i = 0; i<HexGrid<TileType>.EvenOffsets.Length; i++)
            result.UnionWith(GetResultInLine(i, x, y, gameBoard));

        return result;
    }

    HashSet<(int x, int y)>  GetResultInLine(int directionIndex, int x, int y, GameBoard gameBoard)
    {
        var even = HexGrid<TileType>.EvenOffsets;
        var odd = HexGrid<TileType>.OddOffsets;

        TileType tile;
        var result = new HashSet<(int x, int y)>();
        do
        {
            var offset = y % 2 == 0 ? even : odd;
            x += offset[directionIndex].x;
            y += offset[directionIndex].y;
            tile = gameBoard.GetTileAt(x, y);

            if(tile != this && gameBoard.Buildings.ContainsKey((x, y)))
                result.Add((x, y));
        }
        while(tile != null && tile != this);

        if(tile == null)
            return new HashSet<(int x, int y)>();

        return result;
    }
}
