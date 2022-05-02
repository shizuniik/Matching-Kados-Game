using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio; 
using UnityEngine;

[System.Serializable]
public class Sound 
{
    public string name;
    
    public AudioClip clip; 

    [HideInInspector] 
    public AudioSource source;
    
    [Range(0f, 1f)]
    public float volume;

    public bool loop;

    [Range(0.1f, 3f)]
    public float pitch; 
}
