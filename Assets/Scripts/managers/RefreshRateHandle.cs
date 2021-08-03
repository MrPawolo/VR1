using UnityEngine;
using UnityEngine.XR;
public class RefreshRateHandle : MonoBehaviour
{
    private void Start()
    {
        SetRefreshRate();
    }
    public void SetRefreshRate()
    {
        float refreshRate = XRDevice.refreshRate;
        float timeStep;
        if (refreshRate != 0)
        {
            timeStep = 1f / refreshRate;
        }
        else
        {
            timeStep = 1f / 90f;
        }
        Time.fixedDeltaTime = timeStep;
        Debug.Log("Refresh rate was set to: " + refreshRate);
    }
}
