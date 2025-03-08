using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Radio : BeingTriggered
{
    public AudioSource audioSource;
    public AudioClip noise;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public override void TriggerIt()
    {
        audioSource.volume = 0.75f;
        audioSource.clip = noise;
        audioSource.Play();
    }
}

