using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _header;
    [SerializeField] private TextMeshProUGUI _body;

    public void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void SetText(string header, string body)
    {
        _body.text = body;
        _header.text = header;
    }
}
