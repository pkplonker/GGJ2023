using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    public static AudioController instance { get; private set; }
    [SerializeField] private AudioClip audioClip;
    private AudioSource source;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null) clip = audioClip;
        source.PlayOneShot(clip);
    }
}