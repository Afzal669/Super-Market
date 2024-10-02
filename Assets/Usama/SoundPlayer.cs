using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioClip UiAudio;     // Reference to the UI AudioClip
    public AudioSource SoundAudio; // Reference to the AudioSource component

    // Method to play the UI sound
    public void PlayUIAudioSound()
    {
        // Assign the UiAudio clip to the AudioSource
        SoundAudio.clip = UiAudio;

        // Play the audio clip
        SoundAudio.Play();
    }
}
