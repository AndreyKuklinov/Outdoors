
using UnityEngine;

public class SeededGameLoader : MonoBehaviour
{
    [SerializeField] GameSeedHolder _gameSeedHolder;

    public void LoadGame()
    {
        SceneNavigator.LoadGame(_gameSeedHolder.Seed, false);
    }
}