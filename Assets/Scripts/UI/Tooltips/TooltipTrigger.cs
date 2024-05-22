using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Manager.Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Manager.Hide();
    }
}
