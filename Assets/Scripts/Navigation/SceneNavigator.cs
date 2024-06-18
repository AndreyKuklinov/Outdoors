using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneNavigator
{
    public static int ActualGameSeed { get; private set; }
    public static string InputSeed { get; private set; }

    public static bool IsDaily { get; private set; }

    public static void LoadGame(string seed, bool isDaily)
    {
        InputSeed = seed;
        ActualGameSeed = GetActualSeed(seed);
        IsDaily = isDaily;
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

    static int GetActualSeed(string inputSeed)
    {
        if(int.TryParse(inputSeed, out var res))
            return res;
        
        return inputSeed.GetHashCode();
    }
}
