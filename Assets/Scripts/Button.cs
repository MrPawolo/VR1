using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public float thresholdDist = 0.02f;
    public AudioClip pressSound;
    public AudioClip releaseSound;

    public UnityEvent onButtonPressed;
    public UnityEvent onButtonReleased;

    float startPos;
    bool pressed;
    private void OnEnable()
    {
        startPos = transform.localPosition.y;
    }
    private void FixedUpdate()
    {
        float actDelta = startPos - transform.localPosition.y;
        if (!pressed)
        {
            if(actDelta > thresholdDist)
            {
                ButtonWasPressed();
            }
        }
        else
        {
            if (actDelta < thresholdDist)
            {
                ButtonWasReleased();
            }
        }
    }
    void ButtonWasPressed()
    {
        pressed = true;
        onButtonPressed?.Invoke();
        if (pressSound)
        {
            AudioSource.PlayClipAtPoint(pressSound, this.transform.position, StaticConfig.buttonPressVolume);
        }
    }
    void ButtonWasReleased()
    {
        pressed = false;
        onButtonReleased?.Invoke();
        if (releaseSound)
        {
            AudioSource.PlayClipAtPoint(releaseSound, this.transform.position, StaticConfig.buttonReleaseVolume);
        }
    }
}
