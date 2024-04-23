using System;
using TMPro;
using UnityEngine;

public class ResourceText : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameState gameState;
    [SerializeField] private TileType tileType;
    
    #endregion

    #region MonoBehaviour Callbacks

    private void Update()
    {
        text.text = gameState.GetResource(tileType).ToString();
    }

    #endregion
}