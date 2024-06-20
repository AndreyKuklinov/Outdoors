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
        _buildingCollector.BuildingCollectedPoints += ((int x, int y, int sum) args) => SpawnForPopulation(args.x, args.y, args.sum);
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
                SpawnTextAtHex(pos.x, pos.y, "+1<sprite name=prod>", _slowFleetingText);
            }
        }
    }

    void SpawnForPopulation(int x, int y, int sum)
    {
        var sign = sum > 0 ? "+" : "";
        var text = $"{sign}{sum}<sprite name=pop>";
        SpawnTextAtHex(x, y, text, _slowFleetingText);
    }
}
