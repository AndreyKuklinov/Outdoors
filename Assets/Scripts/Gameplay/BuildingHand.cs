using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingHand : MonoBehaviour
{
    [field: SerializeField] public GameBoard GameBoard { get; private set; }
    [field: SerializeField] public TileExplorer TileExplorer { get; private set; }
    [field: SerializeReference] public BuildingHandElement HandElementPrefab { get; private set; }
    [field: SerializeField] public BuildingType TestBuilding { get; private set; }
    [field: SerializeField] public List<BuildingType> BuildingTypes { get; private set; }
    [field: SerializeField] public int MaxHandCapacity { get; private set; }
    [field: SerializeField] public bool IsTestingModeOn { get; private set; }
    [field: SerializeField] public bool IsRandomBuildingModeOn { get; private set; }
    [field: SerializeField] public bool IsBuildingWithoutTerrainProhibited { get; private set; }

    private List<BuildingHandElement> _handElements = new List<BuildingHandElement>();
    private BuildingHandElement _selectedBuilding;

    void Start()
    {
        TileExplorer.TileExplored += TileExplored;
    }

    void AddBuildingToHand(BuildingType buildingType)
    {
        if(_handElements.Count >= MaxHandCapacity)
            return;

        var handElement = Instantiate(HandElementPrefab, this.transform);
        _handElements.Add(handElement);
        handElement.SetType(buildingType);
        handElement.BuildingSelected += BuildingSelected;
        handElement.BuildingDeselected += BuildingDeselected;
    }

    void PlaceBuilding(int x, int y)
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
        Destroy(_selectedBuilding.gameObject);
        _handElements.Remove(_selectedBuilding);
        _selectedBuilding = null;
    }

    void TileExplored(TileType tile)
    {
        if(IsTestingModeOn)
        {
            AddBuildingToHand(TestBuilding);
            return;
        }

        if(IsRandomBuildingModeOn)
        {
            AddBuildingToHand(BuildingTypes[UnityEngine.Random.Range(0, BuildingTypes.Count)]);
            return;
        }

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

    void Update()
    {
        var worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var cellPos = GameBoard.WorldToCell(worldPoint);

        if (Input.GetMouseButtonDown(0))
        {
            PlaceBuilding(cellPos.x, cellPos.y);
        }
    }
}
