using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine;
using VR.Base;

public class Destructable : MonoBehaviour, IDamage
{
    [SerializeField] float maxHealth = 2000;
    [Tooltip("From destroyed to not destroyed object ")]
    [SerializeField] GameObject[] levelsOfDestroy;

    public UnityEvent OnObjDestroy;

    [SerializeField] float actHealth;
    void Start()
    {
        actHealth = maxHealth;
    }
    public void Damage(float velocity, float mass)
    {
        actHealth -= MyFunctions.CalculateDMG(velocity, mass);
        if (actHealth < 0)
        {
            OnObjDestroy?.Invoke();
            return;
        }
        HandleLevelOfDestroy();
    }
    void HandleLevelOfDestroy()
    {
        int LODcount = levelsOfDestroy.Length;
        if(LODcount == 0) { return; }

        float healthDivision = maxHealth / LODcount;
        int actLOD = (int)Math.Floor(actHealth / healthDivision);

        foreach(GameObject LOD in levelsOfDestroy)
        {
            LOD.SetActive(false);
        }
        levelsOfDestroy[actLOD].SetActive(true);
    }
}
