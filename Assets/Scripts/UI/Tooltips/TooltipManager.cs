using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    [SerializeField] Tooltip _tooltip;

    public void Show()
    {
        _tooltip.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _tooltip.gameObject.SetActive(false);
    }
}
