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
        UpdatePivot();
        UpdatePosition();
    }

    public void SetText(string header, string body)
    {
        _body.text = body;
        _header.text = header;
    }

    void UpdatePivot()
    {
        var pos = Input.mousePosition;
        var rt = (RectTransform)(transform);
        var width = rt.rect.width * rt.lossyScale.x;
        var height = rt.rect.height * rt.lossyScale.y;

        var x = pos.x < width ? 0 : 1;
        var y = pos.y < height ? 0 : 1;

        rt.pivot = new Vector2(x, y);
    }

    void UpdatePosition()
    {
        transform.position = Input.mousePosition;
    }
}
