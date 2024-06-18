using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TileDetailer : MonoBehaviour
{
    [SerializeField] private GameBoard _gameBoard;

    private Dictionary<(int x, int y), GameObject> _tileDetails;

    void Awake()
    {
        _gameBoard.TileDrawn += TileDrawn;
        _tileDetails = new Dictionary<(int x, int y), GameObject>();
    }

    void TileDrawn(int x, int y, TileType tileType)
    {
        if(_tileDetails.ContainsKey((x, y)))
            RemoveAnimatedTile(x, y);

        PlaceAnimatedTile(x, y, tileType);
    }

    void PlaceAnimatedTile(int x, int y, TileType tileType)
    {
        if(tileType.AnimatedObject == null)
            return;

        var pos = _gameBoard.CellToWorld(x, y);
        _tileDetails[(x, y)] = Instantiate(tileType.AnimatedObject, pos, quaternion.identity, transform);
    }

    void RemoveAnimatedTile(int x, int y)
    {
        Destroy(_tileDetails[(x, y)]);
    }
}
