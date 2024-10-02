using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Mixers")]
    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;

    [Header("UI Elements")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Mixer Parameters")]
    public string musicVolumeParam = "MusicVolume";
    public string sfxVolumeParam = "SFXVolume";

    private float maxVolume = 0f;
    private float minVolume = -80f; // Minimum volume (mute)

    private const string musicPrefKey = "MusicVolumePref";
    private const string sfxPrefKey = "SFXVolumePref";

    private void Start()
    {
        // Load saved volume values from PlayerPrefs
        float savedMusicVolume = PlayerPrefs.GetFloat(musicPrefKey, 1f);  // Default is 1 (full volume)
        float savedSFXVolume = PlayerPrefs.GetFloat(sfxPrefKey, 1f);      // Default is 1 (full volume)

        // Set slider values and volume from saved preferences
        musicSlider.value = savedMusicVolume;
        sfxSlider.value = savedSFXVolume;

        SetMusicVolume(savedMusicVolume);
        SetSFXVolume(savedSFXVolume);

        // Add listeners to sliders to handle changes
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    // Set the music volume based on slider value and save it
    public void SetMusicVolume(float sliderValue)
    {
        //float volume = Mathf.Lerp(minVolume, maxVolume, sliderValue);  // Convert slider value (0-1) to volume (-80 to 0)
        float volume = sliderValue; // Convert slider value (0-1) to volume (-80 to 0)
        musicMixer.SetFloat(musicVolumeParam, volume);

        // Save the slider value to PlayerPrefs
        PlayerPrefs.SetFloat(musicPrefKey, sliderValue);
    }

    // Set the SFX volume based on slider value and save it
    public void SetSFXVolume(float sliderValue)
    {
        //float volume = Mathf.Lerp(minVolume, maxVolume, sliderValue);  // Convert slider value (0-1) to volume (-80 to 0)
        float volume = sliderValue;
        sfxMixer.SetFloat(sfxVolumeParam, volume);

        // Save the slider value to PlayerPrefs
        PlayerPrefs.SetFloat(sfxPrefKey, sliderValue);
    }

    private void OnApplicationQuit()
    {
        // Ensure PlayerPrefs is saved when the application is closed
        PlayerPrefs.Save();
    }
}
