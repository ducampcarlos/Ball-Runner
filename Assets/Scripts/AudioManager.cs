using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer; // Asignalo desde el Inspector

    private const string SFX_PARAM = "SFX";
    private const string MUSIC_PARAM = "Music";

    private bool isSFXMuted = false;
    private bool isMusicMuted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (audioMixer == null)
        {
            Debug.LogError("AudioMixer no está asignado en el Inspector.");
            return;
        }

        // Restaurar estado guardado
        isSFXMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
        isMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;

        SetVolume(SFX_PARAM, isSFXMuted);
        SetVolume(MUSIC_PARAM, isMusicMuted);
    }

    public void ToggleSFX()
    {
        isSFXMuted = !isSFXMuted;
        PlayerPrefs.SetInt("SFXMuted", isSFXMuted ? 1 : 0);
        SetVolume(SFX_PARAM, isSFXMuted);
    }

    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;
        PlayerPrefs.SetInt("MusicMuted", isMusicMuted ? 1 : 0);
        SetVolume(MUSIC_PARAM, isMusicMuted);
    }

    private void SetVolume(string parameterName, bool mute)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat(parameterName, mute ? -80f : 0f);
        }
    }
}
