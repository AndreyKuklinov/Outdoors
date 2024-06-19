using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioPlayer _audioPlayer;
    public static SoundManager Singleton;

    void Start()
    {
        if(Singleton != null)
        {
            Destroy(gameObject);
            return;
        }

        Singleton = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayClip(AudioClip clip)
    {
        Instantiate(_audioPlayer).PlayClip(clip);
    }
}