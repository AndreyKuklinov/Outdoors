using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FleetingText : MonoBehaviour
{
    [SerializeField] private float _moveUpSpeed;
    [SerializeField] private float _fadeOutSpeed;
    [SerializeField] TextMeshProUGUI _text;

    private Color _newColor;

    void Start()
    {
        _newColor = new Color(_text.color.r, _text.color.g, _text.color.b, 0);
    }

    void Update()
    {
        _text.color = Color.Lerp(_text.color, _newColor, _fadeOutSpeed * Time.deltaTime);
    
        var newPos = new Vector3(transform.position.x, transform.position.y + _moveUpSpeed * Time.deltaTime, transform.position.z);
        transform.position = newPos;

        if(_text.color.a <= 0.01)
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
