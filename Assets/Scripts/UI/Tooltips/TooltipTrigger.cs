using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Text { get; set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Manager.Show(Text);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Manager.Hide();
    }
}
