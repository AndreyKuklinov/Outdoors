using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TileAnimator : MonoBehaviour
{
    [SerializeField] private GameBoard _gameBoard;

    private Dictionary<(int x, int y), GameObject> _animatedTiles;

    void Start()
    {
        _gameBoard.TileDrawn += TileDrawn;
        _animatedTiles = new Dictionary<(int x, int y), GameObject>();
    }

    void TileDrawn(int x, int y, TileType tileType)
    {
        if(_animatedTiles.ContainsKey((x, y)))
            RemoveAnimatedTile(x, y);

        PlaceAnimatedTile(x, y, tileType);
    }

    void PlaceAnimatedTile(int x, int y, TileType tileType)
    {
        if(tileType.AnimatedObject == null)
            return;

        var pos = _gameBoard.CellToWorld(x, y);
        _animatedTiles[(x, y)] = Instantiate(tileType.AnimatedObject, pos, quaternion.identity, transform);
    }

    void RemoveAnimatedTile(int x, int y)
    {
        Destroy(_animatedTiles[(x, y)]);
    }
}
