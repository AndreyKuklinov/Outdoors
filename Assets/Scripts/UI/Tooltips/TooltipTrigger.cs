using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string _body;
    private string _header;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Manager.Show(_header, _body);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Manager.Hide();
    }

    public void SetText(string body, string header)
    {
        _body = body;
        _header = header;
    }
}
