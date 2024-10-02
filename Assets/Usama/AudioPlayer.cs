using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance;
    [Header("Audio Sources")]
    public AudioSource musicSource;     // Audio source for music
    public AudioSource sfxSource;       // Audio source for sound effects

    [Header("Audio Clips")]
    public List<AudioClip> audioClips;  // List of AudioClips to assign in Inspector

    private Dictionary<string, AudioClip> audioClipDictionary;
    private void Awake()
    {
        Instance = this;
        // Initialize the dictionary
        audioClipDictionary = new Dictionary<string, AudioClip>();

        // Populate the dictionary with clip names and clips
        foreach (AudioClip clip in audioClips)
        {
            if (!audioClipDictionary.ContainsKey(clip.name))
            {
                audioClipDictionary.Add(clip.name, clip);
            }
        }
    }

    /// <summary>
    /// Play music by name
    /// </summary>
    /// <param name="clipName">Name of the music clip</param>
    public void PlayMusic(string clipName)
    {
        if (audioClipDictionary.ContainsKey(clipName))
        {
            musicSource.clip = audioClipDictionary[clipName];
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music clip not found: " + clipName);
        }
    }

    /// <summary>
    /// Play sound effect by name
    /// </summary>
    /// <param name="clipName">Name of the sound effect clip</param>
    public void PlaySoundEffect(string clipName)
    {
        if (audioClipDictionary.ContainsKey(clipName))
        {
            sfxSource.PlayOneShot(audioClipDictionary[clipName]);
        }
        else
        {
            Debug.LogWarning("Sound effect clip not found: " + clipName);
        }
    }

    /// <summary>
    /// Stop currently playing music
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
    }
}
