using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR.Base;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] bool iAmDestructable = true;
    Rigidbody myRb;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!myRb) 
        {
            myRb = GetComponent<Rigidbody>();
        }
        else
        {
            VRInteractableBase interactableBase = GetComponent<VRInteractableBase>(); //not very efficient
            float velocity = myRb.velocity.magnitude;
            if (interactableBase && interactableBase.VRHandInteractor)
            {
                velocity = interactableBase.VRHandInteractor.AvgVelocity;
            }
            float mass = myRb.mass;

            if (velocity > StaticConfig.damageVelocityMagnitudeThreshold)
            {
                IDamage damage = collision.gameObject.GetComponent<IDamage>();
                if (damage != null)
                {
                    damage.Damage(velocity, mass);
                }

                if (!iAmDestructable) return; //For example hand and buildings cant deal damage to themselfs
                IDamage destructable = GetComponent<IDamage>();
                if (destructable != null)
                {
                    destructable.Damage(velocity, mass);
                }
            }
        }
    }
}
