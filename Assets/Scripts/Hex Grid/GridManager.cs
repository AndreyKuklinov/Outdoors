using System;
using RedBjorn.ProtoTiles;
using RedBjorn.ProtoTiles.Example;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private MapSettings mapSettings;
    [SerializeField] private MapView mapView;
    
    private MapEntity _mapEntity;

    #region MonoBehaviour Callbacks

    private void Start()
    {
        if (!mapView || !mapSettings)
            throw new NullReferenceException("One or more fields not set");
        
        _mapEntity = new MapEntity(mapSettings, mapView);
        mapView.Init(_mapEntity);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var clickPos = MyInput.GroundPosition(_mapEntity.Settings.Plane());
            var tile = _mapEntity.Tile(clickPos);
            if (tile != null)
            {
                Debug.Log(tile);
            }
        }
    }

    #endregion
}