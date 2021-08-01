using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomPitchSound : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip grabClip;
    [Range(0,1)]public float vol = 1;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        float pitch = Random.Range(0.95f, 1.05f);
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(grabClip, vol);
    }
}
