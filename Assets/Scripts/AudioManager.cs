using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource jumpSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleMusic()
    {
        ToggleAudio(backgroundMusic);
    }
    
    public void ToggleSFX()
    {
        ToggleAudio(jumpSound);
    }

    public void ToggleAudio(AudioSource audio)
    {
        audio.enabled = !audio.enabled;
    }
}
