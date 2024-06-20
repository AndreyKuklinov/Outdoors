using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class FleetingTextSpawner : MonoBehaviour
{
    [SerializeField] FleetingText _quickFleetingText;
    [SerializeField] FleetingText _slowFleetingText;
    [SerializeField] GameBoard _gameBoard;
    [SerializeField] Canvas _canvas;
    [SerializeField] BuildingPointsCollector _buildingCollector;
    [SerializeField] TileExplorer _tileExplorer;

    void Start()
    {
        _tileExplorer.CantExploreInland += ((int x, int y) pos) => SpawnTextAtHex(pos.x, pos.y, "Already explored", _quickFleetingText);
        _tileExplorer.CantExploreNoMoney += ((int x, int y) pos) => SpawnTextAtHex(pos.x, pos.y, "No money", _quickFleetingText);
        _gameBoard.BuildingPlaced += SpawnForEachProductionTile;
        _gameBoard.BuildingPlaced += SpawnForEachPopulationTile;
    }

    public void SpawnTextAtHex(int x, int y, string text, FleetingText prefab)
    {
        var coords = _gameBoard.CellToWorld(x, y);
        var screenPos = Camera.main.WorldToScreenPoint(coords);
        var fleetingText = Instantiate(prefab, _canvas.transform);
        fleetingText.SetText(text);
        fleetingText.transform.position = screenPos;
    }

    void SpawnForEachProductionTile(int x, int y, BuildingType buildingType)
    {
        var adjacents = _gameBoard.GetRevealedPositionsInRange(x, y, 1);
        foreach(var pos in adjacents)
        {
            if(_gameBoard.GetTileAt(pos.x, pos.y) == buildingType.AdjacencyBonusTerrainType)
            {
                SpawnTextAtHex(pos.x, pos.y, "<sprite name=prod>", _slowFleetingText);
            }
        }
    }

    void SpawnForEachPopulationTile(int x, int y, BuildingType buildingType)
    {
        foreach(var pos in buildingType.GetScoringPositionsAfterBuild(x, y, _gameBoard))
        {
            SpawnTextAtHex(pos.x, pos.y, "<sprite name=pop>", _slowFleetingText);
        }
    }
}
