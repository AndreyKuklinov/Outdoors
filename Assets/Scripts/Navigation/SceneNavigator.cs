using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneNavigator
{
    public static int GameSeed { get; private set; }

    public static void LoadGame(int seed)
    {
        GameSeed = seed;
        SceneManager.LoadScene("GameScene");
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void Quit()
    {
        Application.Quit();
    }
}
