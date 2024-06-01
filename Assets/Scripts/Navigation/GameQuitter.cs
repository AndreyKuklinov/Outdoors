using System;
using UnityEngine;

public class GameQuitter : MonoBehaviour
{
    public void QuitGame()
    {
        SceneNavigator.Quit();
    }
}
