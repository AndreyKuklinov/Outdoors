using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public abstract class TileType : ScriptableObject
{
    [field: SerializeField] public Tile TileBase {get; private set;}
    
    [TextArea]
    public string TooltipText;

    public abstract bool CanBeBuiltUpon { get; }
    public abstract BuildingType ExplorationBuildingType { get; }
}
