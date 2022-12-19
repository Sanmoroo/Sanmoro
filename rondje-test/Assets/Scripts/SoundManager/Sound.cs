using UnityEngine.Audio;
using UnityEngine;

// This class describes an audio clip that we want to add and makes the below options available to change on that clip
[System.Serializable]
public class Sound 
{
    public string name;
    public AudioClip clip;

    //Range makes it a slider :3
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    //Not sure what this does ;-; 
    [HideInInspector]
    public AudioSource source;
}
