using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            if(s.Spacial) s.source.spatialBlend = 1; else s.source.spatialBlend = 0;
            s.source.playOnAwake = s.playOnAwake;
        }
    }

    public void Play(string name)
    {
        //foreach(Sound s in sounds)
        //{
        //    s.source.Play();
        //}
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s.source.isPlaying) return;
        s.source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
