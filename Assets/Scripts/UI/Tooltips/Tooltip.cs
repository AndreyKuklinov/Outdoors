using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;

    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void SetText(string text)
    {
        _textMesh.text = text;
    }
}
