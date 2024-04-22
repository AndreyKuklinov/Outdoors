using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : ScriptableObject
{
    public int Money { get; private set; }
    
    public void Reset()
    {
        Money = 0;
    }
}
