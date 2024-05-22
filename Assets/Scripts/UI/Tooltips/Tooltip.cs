using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour
{
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
