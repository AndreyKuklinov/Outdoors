using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private float dragSpeed = 1;
    private Vector3 _dragOrigin;

    #endregion

    #region MonoBehaviour Callbacks
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _dragOrigin = Input.mousePosition;
            return;
        }
 
        if (!Input.GetMouseButton(0)) return;
 
        Vector3 pos = Camera.main.ScreenToViewportPoint(_dragOrigin - Input.mousePosition);
        Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
 
        transform.Translate(move, Space.World);  
    }

    #endregion
}