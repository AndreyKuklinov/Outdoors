using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Header;
    
    [TextArea]
     public string Body;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Manager.Show(Header, Body);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Manager.Hide();
    }

    public void SetText(string body, string header)
    {
        Body = body;
        Header = header;
    }
}
