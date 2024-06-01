using TMPro;
using UnityEngine;

public class ScoreLabel : MonoBehaviour
{
    [SerializeField] private TerrainPointsCollector _terrainCollector;
    [SerializeField] private BuildingPointsCollector _buildingsCollector;

    private TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _text.text = $"SCORE: {_terrainCollector.PointsTotal * _buildingsCollector.PointsTotal}";
    }
}
