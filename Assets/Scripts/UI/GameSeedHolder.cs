using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSeedHolder : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;

    public int Seed
        => _inputField.text.GetHashCode();

    void Start()
    {
        PickRandomSeed();
    }

    public void PickRandomSeed()
    {
        _inputField.text = Random.Range(0, int.MaxValue).ToString(); 
    }
}
