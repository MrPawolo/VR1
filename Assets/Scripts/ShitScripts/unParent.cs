using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unParent : MonoBehaviour
{

    public Transform transformToUnparent;
    public void UnParent()
    {
        transformToUnparent.SetParent(null);
    }
}
