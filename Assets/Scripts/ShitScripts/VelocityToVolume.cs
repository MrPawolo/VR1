using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VelocityToVolume : MonoBehaviour
{
    AudioSource audioSource;
    Rigidbody myRb;
    public AnimationCurve velocityToVolume;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        myRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        audioSource.volume = velocityToVolume.Evaluate(myRb.velocity.magnitude);
    }
}
