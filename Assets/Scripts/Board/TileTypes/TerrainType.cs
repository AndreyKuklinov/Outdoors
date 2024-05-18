using UnityEngine;

[CreateAssetMenu]
public class TerrainType : TileType
{
    public override bool CanBeBuiltUpon => _canBeBuiltUpon;
    public override BuildingType ExplorationBuildingType => _explorationBuildingType;

    [SerializeField] private bool _canBeBuiltUpon;
    [SerializeField] private BuildingType _explorationBuildingType;
}
