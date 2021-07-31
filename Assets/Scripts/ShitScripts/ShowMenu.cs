using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMenu : MonoBehaviour
{
    public Transform head;
    public float distanceFromPlayer = 0.5f;
    public GameObject menuRoot;

    public void TryToShow()
    {
        if (!menuRoot.activeSelf)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    void Show()
    {
        Transform cameraTrans = head;
        if (head == null)
        {
            cameraTrans = Camera.main.transform;
        }
        Vector3 rightVec = cameraTrans.right;
        Vector3 forwardDir = Vector3.Cross(rightVec, Vector3.up);
        Vector3 projectedForwardDir = Vector3.ProjectOnPlane(forwardDir, Vector3.up);
        menuRoot.transform.position = cameraTrans.position + projectedForwardDir * distanceFromPlayer;
        menuRoot.transform.LookAt(cameraTrans, Vector3.up);
        menuRoot.SetActive(true);
    }
    void Hide()
    {
        menuRoot.SetActive(false);
    }
}
