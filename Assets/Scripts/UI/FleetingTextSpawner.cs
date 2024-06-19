using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetingTextSpawner : MonoBehaviour
{
    [SerializeField] GameObject _fleetingTextPrefab;
    [SerializeField] GameBoard _gameBoard;
    [SerializeField] Canvas _canvas;

    public void SpawnTextAtHex(int x, int y)
    {
        var coords = _gameBoard.CellToWorld(x, y);
        var screenPos = Camera.main.WorldToScreenPoint(coords);
        var text = Instantiate(_fleetingTextPrefab, _canvas.transform);
        text.transform.position = screenPos;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SpawnTextAtHex(0, 0);
        }
    }
}
