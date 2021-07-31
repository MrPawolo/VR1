using UnityEngine;
using UnityEngine.XR;
public class RefreshRateHandle : MonoBehaviour
{
    private void Awake()
    {
        float refreshRate = XRDevice.refreshRate;
        float timeStep;
        if(refreshRate != 0)
        {
            timeStep = 1 / refreshRate;
        }
        else
        {
            timeStep = 1 / 90;
        }
        Time.fixedDeltaTime = timeStep;
        Debug.Log("Refresh rate was set to: " + refreshRate);
    }
}
