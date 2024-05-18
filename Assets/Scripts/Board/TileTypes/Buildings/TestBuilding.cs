using UnityEngine;

[CreateAssetMenu(menuName="BuildingTypes/TestBuilding")]
public class TestBuilding : BuildingType
{
    public override int CalculateScore(int x, int y, GameBoard gameBoard) {
        return 1;
    }
}
