using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] TileRaycaster _raycaster;
    [SerializeField] TileExplorer _tileExplorer;
    [SerializeField] BuildingHand _buildingHand;

    private Vector3 _posWhenBtnDown;

    void Update()
    {
        HandleMouseDown();
        HandleMouseUp();
    }

    void HandleMouseDown()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        _posWhenBtnDown = Input.mousePosition;
    }

    void HandleMouseUp()
    {
        if (!Input.GetMouseButtonUp(0))
            return;

        if(_posWhenBtnDown.Equals(Input.mousePosition))
        {
            if(_buildingHand.IsBuildingSelected)
                PlaceBuilding();

            else
                ExploreTile();
        }
    }

    void ExploreTile()
    {
        var tilePos = _raycaster.GetTilePosition(Input.mousePosition);
        _tileExplorer.ExploreTile(tilePos.x, tilePos.y);
    }

    void PlaceBuilding()
    {
        var tilePos = _raycaster.GetTilePosition(Input.mousePosition);
        _buildingHand.PlaceBuilding(tilePos.x, tilePos.y);
    }
}