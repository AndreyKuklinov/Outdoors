using System;
using UnityEngine;

public class DailyChallengeLoader : MonoBehaviour
{
    public static int CurrentDateHash
        => DateTime.Now.Date.GetHashCode();

    public void LoadDailyChallenge()
    {
        SceneNavigator.LoadGame(CurrentDateHash.ToString(), true);
    }
}