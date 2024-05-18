using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName="BuildingTypes/InLineWithItsTypeBuilding")]
public class InLineWithItsTypeBuilding : BuildingType
{
    public override int CalculateScore(int x, int y, GameBoard gameBoard)
    {
        var score = 0;
        for(var i = 0; i<HexGrid<TileType>.EvenOffsets.Length; i++)
            score += GetScoreInLine(i, x, y, gameBoard);

        return score;
    }

    int GetScoreInLine(int directionIndex, int x, int y, GameBoard gameBoard)
    {
        var even = HexGrid<TileType>.EvenOffsets;
        var odd = HexGrid<TileType>.OddOffsets;

        TileType tile;
        var count = 0;
        do
        {
            var offset = y % 2 == 0 ? even : odd;
            x += offset[directionIndex].x;
            y += offset[directionIndex].y;
            tile = gameBoard.GetTileAt(x, y);

            if(tile != this && gameBoard.Buildings.ContainsKey((x, y)))
                count++;
        }
        while(tile != null && tile != this);

        if(tile == null)
            return 0;

        return count;
    }
}
