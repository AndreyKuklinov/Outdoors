using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip _menuMusic;
    [SerializeField] AudioClip _gameMusic;
    [SerializeField] AudioSource _audioSource; 

    public static MusicManager Singleton;
    private bool _isInGame;

    void Start()
    {
        if(Singleton != null)
        {
            Destroy(gameObject);
            return;
        }

        Singleton = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += SceneChanged;
    }

    void SceneChanged(Scene prev, Scene current)
    {
        if(current.name == "GameScene")
        {
            _audioSource.Stop();
            _audioSource.clip = _gameMusic;
            _audioSource.PlayDelayed(10);
            _isInGame = true;
        }

        if(current.name == "MainMenu" && _isInGame)
        {
            _audioSource.Stop();
            _audioSource.clip = _menuMusic;
            _audioSource.Play();
            _isInGame = false;
        }
    }
}
