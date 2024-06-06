using UnityEngine;
using UnityEngine.EventSystems;

public class CursorTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _isCursorOverObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isCursorOverObject = true;
        CursorManager.Singleton.SetCursorOverClicklableUI(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isCursorOverObject = false;
        CursorManager.Singleton.SetCursorOverClicklableUI(false);
    }

    void OnDisable()
    {
        if(_isCursorOverObject)
            CursorManager.Singleton.SetCursorOverClicklableUI(false);

        _isCursorOverObject = false;
    }
}
