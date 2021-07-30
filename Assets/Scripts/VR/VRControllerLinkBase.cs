using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR.IO
{
    public class VRControllerLinkBase : MonoBehaviour
    {
        public virtual Vector3 GetControllerLocalPostion()
        {
            return Vector3.zero;
        }
        public virtual Quaternion GetControllerLocalRotation()
        {
            return Quaternion.identity;
        }
        public virtual float GetTriggerVal()
        {
            return 0.0f;
        }
        public virtual bool GetTriggerTouch()
        {
            return false;
        }
        public virtual float GetGripVal()
        {
            return 0.0f;
        }
        public virtual bool GetGripTouch()
        {
            return false;
        }
        public virtual Vector2 GetJoyStickVal()
        {
            return Vector2.zero;
        }
        public virtual bool GetJoyStickClick()
        {
            return false;
        }
        public virtual bool GetJoyStickTouch()
        {
            return false;
        }
        public virtual bool GetFirstButton()
        {
            return false;
        }
        public virtual bool GetFirstButtonTouch()
        {
            return false;
        }
        public virtual bool GetSecondButton()
        {
            return false;
        }
        public virtual bool GetSecondButtonTouch()
        {
            return false;
        }
        public virtual void SendHapticImpulse(float duration, float amount)
        {

        }
        public virtual Vector3 GetVelocity()
        {
            return Vector3.zero;
        }
        public virtual Vector3 GetAngularVelocity()
        {
            return Vector3.zero;
        }
    }
}
