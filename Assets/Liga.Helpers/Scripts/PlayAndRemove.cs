using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayAndRemove : MonoBehaviour
{
    AudioSource audioSource;

    public bool IsPlaying { get { return audioSource.isPlaying; } }

    public void Play(AudioClip audioClip, bool loop, float volume, float duration, float pitch)
    {
        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.clip = audioClip;
        audioSource.loop = loop;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();

        Invoke("RemoveAudio", duration == 0 ? audioClip.length : duration);
    }

    void RemoveAudio()
    {
        audioSource.Stop();
        //GameObject.Destroy(gameObject);
    }
}
