using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeButtonPress : MonoBehaviour
{
    public float time = 2f;
    public UnityEvent action;
    IEnumerator waitCorutine;
    public void StartPress()
    {
        Debug.Log("Pressed ");
        if(waitCorutine != null)
        {
            StopCoroutine(waitCorutine);
        }
        waitCorutine = WaitAndAction();
        StartCoroutine(waitCorutine);
    }
    public void StopPress()
    {
        Debug.Log("Released");
        if (waitCorutine != null)
        {
            StopCoroutine(waitCorutine);
            waitCorutine = null;
        }
    }
    IEnumerator WaitAndAction()
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
        waitCorutine = null;
    }
}
