using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
