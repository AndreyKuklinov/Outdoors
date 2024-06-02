using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingHand : MonoBehaviour
{
    [field: SerializeField] public GameBoard GameBoard { get; private set; }
    [field: SerializeField] public TileExplorer TileExplorer { get; private set; }
    [field: SerializeField] public List<BuildingType> BuildingTypes { get; private set; }

    [SerializeField] private List<BuildingHandElement> _handElements;

    [field: SerializeField] public int MaxHandCapacity { get; private set; }
    [field: SerializeField] public bool IsTestingModeOn { get; private set; }
    [field: SerializeField] public bool IsRandomBuildingModeOn { get; private set; }
    [field: SerializeField] public bool IsBuildingWithoutTerrainProhibited { get; private set; }

    [SerializeField] private bool _exploredBuildingsYieldTiles;

    public bool IsBuildingSelected 
        => _selectedBuilding != null;

    private BuildingHandElement _selectedBuilding;
    private int _buildingsCount;

    void Start()
    {
        TileExplorer.TileExplored += TileExplored;

        foreach(var handElement in _handElements)
        {
            handElement.BuildingSelected += BuildingSelected;
            handElement.BuildingDeselected += BuildingDeselected;
        }
    }

    void AddBuildingToHand(BuildingType buildingType)
    {
        if(_buildingsCount >= MaxHandCapacity || buildingType == null)
            return;

        var handElement = _handElements.First(x => !x.gameObject.activeSelf);
        handElement.SetType(buildingType);
        handElement.gameObject.SetActive(true);
        _buildingsCount++;
    }

    public void PlaceBuilding(int x, int y)
    {
        if(_selectedBuilding == null || !GameBoard.RevealedTiles.Contains((x, y)) || !GameBoard.GetTileAt(x, y).CanBeBuiltUpon)
            return;

        var buildingType = _selectedBuilding.BuildingType;

        if(IsBuildingWithoutTerrainProhibited
            && !GameBoard.GetRevealedPositionsInRange(x, y, 1).Any(pos => GameBoard.GetTileAt(pos.x, pos.y) == buildingType.AdjacencyBonusTerrainType))
        {
            return;
        }

        GameBoard.PlaceBuilding(x, y, buildingType);
        RemoveSelectedBuildingFromHand();
    }

    void RemoveSelectedBuildingFromHand()
    {
        _selectedBuilding.gameObject.SetActive(false);
        _selectedBuilding.Deselect();
        _selectedBuilding = null;
        _buildingsCount--;
    }

    void TileExplored(TileType tile)
    {
        if(IsRandomBuildingModeOn)
        {
            AddBuildingToHand(BuildingTypes[UnityEngine.Random.Range(0, BuildingTypes.Count)]);
            return;
        }

        if(!_exploredBuildingsYieldTiles && tile as BuildingType != null)
            return;

        AddBuildingToHand(tile.ExplorationBuildingType);
//        Debug.Log(string.Join(' ', _buildingsInHand));
    }

    void BuildingSelected(object sender, BuildingHandElement handElement)
    {
        _selectedBuilding = handElement;

        foreach(var h in _handElements)
        {
            if(h != handElement)
                h.Deselect();
        }
    }

    void BuildingDeselected(object sender, BuildingHandElement handElement)
    {
        if(_selectedBuilding == handElement)
            _selectedBuilding = null;
    }
}
