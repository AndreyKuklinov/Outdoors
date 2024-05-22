using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingHandElement : MonoBehaviour, IPointerClickHandler
{
    [field:SerializeField] public BuildingType BuildingType { get; private set; }
    [field:SerializeField] public float ScaleWhenSelected { get; private set; }
    [field: SerializeField] public Image Image { get; private set; }

    [SerializeField] private TooltipTrigger _tooltipTrigger;

    public event EventHandler<BuildingHandElement> BuildingSelected;
    public event EventHandler<BuildingHandElement> BuildingDeselected;

    private bool _isSelected = false;

    public void SetType(BuildingType buildingType)
    {
        BuildingType = buildingType;
        Image.sprite = buildingType.TileBase.sprite;
        _tooltipTrigger.Text = buildingType.TooltipText;
    }

    public void Deselect()
    {
        transform.localScale = new Vector3(1f, 1f, 1);
        _isSelected = false;
        OnBuildingDeselected();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!_isSelected)
        {
            Select();
            return;
        }

        Deselect();
    }

    void Select()
    {
        transform.localScale = new Vector3(ScaleWhenSelected, ScaleWhenSelected, 1);
        _isSelected = true;
        OnBuildingSelected();
    }

    void OnBuildingSelected()
    {
        var handler = BuildingSelected;
        if (handler != null)
        {
            handler(this, (this));
        }
    }

    void OnBuildingDeselected()
    {
        var handler = BuildingDeselected;
        if (handler != null)
        {
            handler(this, (this));
        }
    }
}
