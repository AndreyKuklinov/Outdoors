using System;
using System.Collections;
using System.Collections.Generic;
using RedBjorn.Utils;
using UnityEngine;

[CreateAssetMenu(menuName = "Game State")]
public class GameState : ScriptableObject
{
    [SerializeField] private SerializableDictionary<TileType, int> resources;
    public int Coins { get; private set; }

    #region Public Methods
    
    public void ResetState()
    {
        resources = new();
        foreach (var type in Enum.GetValues(typeof(TileType)))
        {
            resources[(TileType)type] = 0;
        }
        Coins = 0;
    }

    public void AddResource(TileType type)
    {
        resources[type]++;
    }

    public void UseResource(TileType type, int amount)
    {
        if (resources[type] < amount)
            throw new ArgumentException("You're trying to spend more resources than you have");
        resources[type] -= amount;
    }

    public int GetResource(TileType type)
    {
        return resources[type];
    }

    #endregion

    #region ScriptableObject Callbacks

    private void OnEnable()
    {
        ResetState();
    }

    #endregion
}
