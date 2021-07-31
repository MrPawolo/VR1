using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR.Base
{
    public class HandDistanceHaptic : MonoBehaviour
    {
        VRController controller;
        VRManager VRManager;
        private void Start()
        {
            controller = GetComponent<VRHandInteractor>().Controller;
            VRManager = GetComponent<VRHandInteractor>().VRManager;
        }
        public void OnCollisionEntered(Collision collision)
        {
            float hitVelSqr = collision.relativeVelocity.magnitude;
            float haptic = VRManager.OnCollisionHaptic.Evaluate(hitVelSqr);
            controller.SendHapticImpulse(0.1f, haptic);
        }
        public void OnCollisionStayed(Collision collision)
        {
            float distSqr = (controller.transform.position - this.transform.position).magnitude;
            float haptic = VRManager.HandDistanceHapticAmount.Evaluate(distSqr);
            controller.SendHapticImpulse(0.1f, haptic);
        }
        private void OnCollisionStay(Collision collision)
        {
            OnCollisionStayed(collision);
        }
        private void OnCollisionEnter(Collision collision)
        {
            OnCollisionEntered(collision);
        }
    }
}
