using UnityEngine;
using UnityEngine.Serialization;

public class CameraDrag : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private float dragSpeed = 1;
    [SerializeField] private Camera mainCamera;
    private Vector3 _dragOrigin;

    #endregion

    #region MonoBehaviour Callbacks
    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _dragOrigin = Input.mousePosition;
            return;
        }
 
        if (!Input.GetMouseButton(1)) return;
 
        var pos = mainCamera.ScreenToViewportPoint(_dragOrigin - Input.mousePosition);
        var move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
 
        transform.Translate(move, Space.World);  
    }

    #endregion
}