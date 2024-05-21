using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
