using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound 
{
    //Additional class the Audiomanager that makes us change the data that is stored within
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
