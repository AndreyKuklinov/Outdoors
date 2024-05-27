using TMPro;
using UnityEngine;

public class ProductionLabel : MonoBehaviour
{
    [SerializeField] private TerrainPointsCollector _pointsCollector;

    private TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _text.text = $"{_pointsCollector.PointsTotal}";
    }
}
