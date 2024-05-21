using TMPro;
using UnityEngine;

public class PopulationLabel : MonoBehaviour
{
    [SerializeField] private BuildingPointsCollector _pointsCollector;

    private TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _text.text = $"Население: {_pointsCollector.PointsTotal}";
    }
}
