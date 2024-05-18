using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileType : ScriptableObject
{
    [field: SerializeField] public Tile TileBase {get; private set;}
    public abstract bool CanBeBuiltUpon { get; }
    public abstract BuildingType ExplorationBuildingType { get; }
}
