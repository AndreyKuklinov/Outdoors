using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneNavigator
{
    public static string GameSeed { get; private set; }

    public static void LoadMainGame(string seed)
    {
        GameSeed = seed;
        SceneManager.LoadScene("GameScene");
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
