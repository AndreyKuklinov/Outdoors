using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TooltipManager _tooltipManager;
 
    public string Header;
    
    [TextArea]
    public string Body;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tooltipManager.Show(Header, Body);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tooltipManager.Hide();
    }

    public void SetText(string body, string header)
    {
        Body = body;
        Header = header;
    }
}
