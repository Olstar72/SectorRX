﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource mainAudio;
    public List<AudioClip> allMusic = new List<AudioClip>();

    private void Start()
    {
        mainAudio.clip = allMusic[0];
        mainAudio.Play();
    }
}
