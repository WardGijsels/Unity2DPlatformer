using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager instance;
    private static AudioSource audioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private int SoundEffectVolume = 5;

    private void Awake()
    {
        // Zorg ervoor dat er maar één instantie van deze klasse is
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        // Haal de SE op en kijk na of het bestaat
        AudioClip audioClip = soundEffectLibrary.GetClip(soundName);
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void SetVolume()
    {
        audioSource.volume = SoundEffectVolume;
    }
}