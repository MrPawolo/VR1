using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine;
using VR.IO;

namespace VR.Base
{
    [AddComponentMenu("ML/VR/Toolkit/XROculusControllerLink")]
    public class XRControllerLink : VRControllerLinkBase
    {
        public XRNode m_ControllerNode;
        public XRNode controllerNode { get { return m_ControllerNode; } set { m_ControllerNode = value; } }
        private InputDevice _InputDevice;
        public InputDevice inputDevice
        {
            get
            {
                return _InputDevice.isValid ? _InputDevice : (_InputDevice = InputDevices.GetDeviceAtXRNode(controllerNode));
            }
        }


        public override Vector3 GetControllerLocalPostion()
        {
            inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 pos);
            return pos;
        }
        public override Quaternion GetControllerLocalRotation()
        {
            inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rot);
            return rot;
        }
        public override float GetTriggerVal()
        {
            inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float val);
            return val;
        }

        public override float GetGripVal()
        {
            inputDevice.TryGetFeatureValue(CommonUsages.grip, out float val);
            return val;
        }
        public override Vector2 GetJoyStickVal()
        {
            inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 pos);
            return pos;
        }
        public override bool GetJoyStickClick()
        {
            inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool state);
            return state;
        }
        public override bool GetJoyStickTouch()
        {
            inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out bool state);
            return state;
        }
        public override bool GetFirstButton()
        {
            inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool state);
            return state;
        }
        public override bool GetFirstButtonTouch()
        {
            inputDevice.TryGetFeatureValue(CommonUsages.primaryTouch, out bool state);
            return state;
        }
        public override bool GetSecondButton()
        {
            inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool state);
            return state;
        }
        public override bool GetTriggerTouch()
        {
            inputDevice.TryGetFeatureValue(CommonUsages.secondaryTouch, out bool state);
            return state;
        }
        public override void SendHapticImpulse(float duration, float amount)
        {
            inputDevice.SendHapticImpulse(0, amount, duration);
        }
        public override Vector3 GetVelocity()
        {
            inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity);
            return velocity;
        }
        public override Vector3 GetAngularVelocity()
        {
            inputDevice.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out Vector3 angularVelocity);
            return angularVelocity;
        }
    }
}
