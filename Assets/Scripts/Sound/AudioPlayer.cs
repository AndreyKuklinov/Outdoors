using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;

    private bool _isActive;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if(_isActive && !_audioSource.isPlaying)
            Destroy(gameObject);
    }

    public void PlayClip(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
        _isActive = true;
    }
}
