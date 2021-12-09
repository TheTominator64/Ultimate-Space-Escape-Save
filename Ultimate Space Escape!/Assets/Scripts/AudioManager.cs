using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
            s.source.loop = s.loop;
        }
    }

    //Allows a song or sound effect to play, can be called via code or when attached to a button
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

       

        s.source.Play();
    }


    public void LowerVolume(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }



        s.source.volume = .05f;
    }

    public void SilenceVolume(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }



        s.source.volume = 0f;
    }
    public void NormalizeVolume(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }



        s.source.volume = .5f;
    }

    //Allows a song or sound to be stopped when this public void is called, via code or when attached to a button
    public void Stop(string Name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == Name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + Name + " not found!");
            return;
        }
        s.source.Stop();

        //Then use      Stop("Theme");       to stop it
    }

}

