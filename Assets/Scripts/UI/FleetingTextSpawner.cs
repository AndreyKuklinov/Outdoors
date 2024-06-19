using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetingTextSpawner : MonoBehaviour
{
    [SerializeField] FleetingText _fleetingTextPrefab;
    [SerializeField] GameBoard _gameBoard;
    [SerializeField] Canvas _canvas;
    [SerializeField] TileExplorer _tileExplorer;

    void Start()
    {
        _tileExplorer.CantExploreInland += ((int x, int y) pos) => SpawnTextAtHex(pos.x, pos.y, "Already explored");
        _tileExplorer.CantExploreNoMoney += ((int x, int y) pos) => SpawnTextAtHex(pos.x, pos.y, "No money");
    }

    public void SpawnTextAtHex(int x, int y, string text)
    {
        var coords = _gameBoard.CellToWorld(x, y);
        var screenPos = Camera.main.WorldToScreenPoint(coords);
        var fleetingText = Instantiate(_fleetingTextPrefab, _canvas.transform);
        fleetingText.SetText(text);
        fleetingText.transform.position = screenPos;
    }


}
