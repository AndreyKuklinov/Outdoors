using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TerrainType : TileType
{
    public override bool CanBeBuiltUpon => _canBeBuiltUpon;
    public override BuildingType ExplorationBuildingType => _explorationBuildingType;
    public Tile FoggyVersion;

    [SerializeField] private bool _canBeBuiltUpon;
    [SerializeField] private BuildingType _explorationBuildingType;
}
