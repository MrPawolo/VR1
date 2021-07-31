using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestoy : MonoBehaviour
{
    private void OnEnable()
    {
        Destroy(gameObject, StaticConfig.destroyTime + Random.Range(-2, 2));
    }
}
