using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="BuildingTypes/TestBuilding")]
public class TestBuilding : BuildingType
{

    public override HashSet<(int x, int y)> GetScoringPositionsAfterBuild(int x, int y, GameBoard gameBoard)
    {
        throw new System.NotImplementedException();
    }
}
