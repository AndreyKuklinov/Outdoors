using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovedDetector : MonoBehaviour
{
    public event Action MouseMoved;

    private Vector3 _prevMousePos;

    void Update()
    {
        var mousePos = Input.mousePosition;

        if(_prevMousePos == mousePos)
            return;

        _prevMousePos = mousePos;
        MouseMoved?.Invoke();
    }
}
