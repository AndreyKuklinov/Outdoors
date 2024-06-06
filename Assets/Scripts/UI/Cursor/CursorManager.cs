using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Singleton;

    [SerializeField] Vector2 hotSpot;
    [SerializeField] Texture2D idleTexture;
    [SerializeField] Texture2D hoverTexture;
    [SerializeField] Texture2D clickTexture;
    
    private bool _isCursorOverClicklableUI;

    public void SetCursorOverClicklableUI(bool value)
    {
        _isCursorOverClicklableUI = value;
    }

    void Start()
    {
        Singleton = this;
    }

    void Update()
    {   
        Cursor.SetCursor(GetCursorTexture(), hotSpot, CursorMode.Auto);
    }

    Texture2D GetCursorTexture()
    {
        if(Input.GetMouseButton(0))
            return clickTexture;

        if(_isCursorOverClicklableUI)
            return hoverTexture;

        return idleTexture;
    }
}