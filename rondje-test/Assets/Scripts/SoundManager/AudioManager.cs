using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Added to the game object
    public Sound[] sounds;
    public static AudioManager instance;
    
    void Awake()
    {
        // Make sure there is an AudioManager object initialized.
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }

        // Options for each sound in the sound list.
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    // Find the clip that matches the name passed as an argument and play it.
    public void Play (string name)
    {
       Sound s = Array.Find(sounds, sound => sound.name == name);

       if (!s.source.isPlaying)
       {
            s.source.Play();
       }
    }
}
