using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraDrag : MonoBehaviour
{
    [SerializeField] GameObject _modalWindow;
    public event Action OnCameraDrag;

#region Variables

    private Vector3 _origin;
    private Vector3 _difference;

    private Camera _mainCamera;

    private bool _isDragging;

#endregion

    private void Awake() => _mainCamera = GetComponent<Camera>();

    public void OnDrag(InputAction.CallbackContext ctx)
    {
        if(_modalWindow.activeSelf)
            return;

        if (ctx.started) _origin = GetMousePosition;
        _isDragging = ctx.started || ctx.performed;
        
    }

    private void LateUpdate()
    {
        if (!_isDragging) return;

        _difference = GetMousePosition - transform.position;
        transform.position = _origin - _difference;
        OnCameraDrag?.Invoke();
    }

    private Vector3 GetMousePosition => _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
}
