using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR.Base;

public class StartGameHandler : MonoBehaviour
{
    public GameObject[] ui;
    public Rigidbody playerRb;
    public VRManager VRManager;
    public Transform startPos;

    public void Start()
    {
        if (VRManager)
        {
            VRManager.TeleportTo(startPos.position);
        }
    }
    public void StartGame()
    {
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.8f);
        foreach (GameObject go in ui)
        {
            go.SetActive(false);
        }
        playerRb.isKinematic = false;
    }
}
