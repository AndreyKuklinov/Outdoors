using System;
using RedBjorn.ProtoTiles;
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

    #endregion
}