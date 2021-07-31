using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestoy : MonoBehaviour
{
    private void OnEnable()
    {
        Rigidbody myRb = GetComponent<Rigidbody>();
        if (myRb)
        {
            float x = Random.Range(-1, 1);
            float z = Random.Range(-1, 1);
            myRb.AddForce(new Vector3(x, 0, z) * StaticConfig.onDestroyForce,ForceMode.VelocityChange);
        }
        Destroy(gameObject, StaticConfig.destroyTime + Random.Range(-2, 2));
    }
}
