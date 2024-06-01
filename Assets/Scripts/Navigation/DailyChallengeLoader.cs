using System;
using UnityEngine;

public class DailyChallengeLoader : MonoBehaviour
{
    public void LoadDailyChallenge()
    {
        var today = DateTime.Now.Date;
        SceneNavigator.LoadGame(today.GetHashCode());
    }
}