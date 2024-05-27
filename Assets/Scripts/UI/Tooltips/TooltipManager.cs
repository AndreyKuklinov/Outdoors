using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Manager;

    [SerializeField] Tooltip _tooltip;

    void Start()
    {
        Manager = this;
    }

    public void Show(string text)
    {
        _tooltip.SetText(text);
        _tooltip.Update();
        _tooltip.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _tooltip.gameObject.SetActive(false);
    }
}
