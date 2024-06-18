using TMPro;
using UnityEngine;

public class SeedLabel : MonoBehaviour
{
    private TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _text.text = $"Seed: {SceneNavigator.InputSeed}";
    }
}
