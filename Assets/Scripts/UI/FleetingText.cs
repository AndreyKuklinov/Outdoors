using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;

public class FleetingText : MonoBehaviour
{
    [SerializeField] private float _moveUpSpeed;
    [SerializeField] private float _fadeOutSpeed;
    [SerializeField] private float _disappearThreshold;
    [SerializeField] TextMeshProUGUI _text;

    private Color _newColor;
    private Vector3 _worldPosition;
    private float _yOffset;

    void Start()
    {
        _newColor = new Color(_text.color.r, _text.color.g, _text.color.b, 0);
        _worldPosition = Camera.main.ScreenToWorldPoint(transform.position);
        _yOffset = 0;
    }

    void Update()
    {
        _text.color = Color.Lerp(_text.color, _newColor, _fadeOutSpeed * Time.deltaTime);
    
        _yOffset += _moveUpSpeed * Time.deltaTime;
        var pos = Camera.main.WorldToScreenPoint(_worldPosition);
        transform.position = new Vector3(pos.x, pos.y + _yOffset, pos.z);

        if(_text.color.a <= _disappearThreshold)
        {
            Destroy(gameObject);
            return;
        }       
    }

    public void SetText(string text)
    {
        _text.text = text;
    }
}
