using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine;
using VR.Base;

public class Destructable : MonoBehaviour, IDamage
{
    [SerializeField] float pointsForDestruction = 10f;
    [SerializeField] bool isBuilding = false;
    [SerializeField] float maxHealth = 100;
    [Tooltip("From destroyed to not destroyed object ")]
    [SerializeField] GameObject[] levelsOfDestroy;

    public UnityEvent OnObjDestroy;
    MeshRenderer meshRenderer;
    [SerializeField] float actHealth;
    void Start()
    {
        actHealth = maxHealth;
        if (isBuilding)
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.SetFloat(StaticConfig.CrackProggres, 0);
        }
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
        if (!isBuilding)
        {
            int LODcount = levelsOfDestroy.Length;
            if (LODcount == 0) { return; }

            float healthDivision = maxHealth / LODcount;
            int actLOD = (int)Math.Floor(actHealth / healthDivision);

            foreach (GameObject LOD in levelsOfDestroy)
            {
                LOD.SetActive(false);
            }
            levelsOfDestroy[actLOD].SetActive(true);
        }
        else
        {
            meshRenderer.material.SetFloat(StaticConfig.CrackProggres, 1 - actHealth/maxHealth );
        }
        
    }
}
