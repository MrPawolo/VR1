using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR.Base
{
    public enum Hand { Left, Right };
    public enum TurnType { Smooth, Snap };
    public enum HoverType { Grip, Trigger};
    public struct JointValues
    {
        public ConfigurableJoint joint;
        public float positionSpring;
        public float positionDumper;
        public float maximumForce;
        public float angularSpring;
        public float angularDumper;
        public float angularMaximumForce;
    }
    public struct MyFunctions
    {
        public static float CalculateDMG(float velocity, float mass)
        {
            return (mass * velocity * velocity) / 2;
        }
        public static float[] AddTo3Velocities(float[] vel, float newVel)
        {
            float[] returnVel = { vel[1], vel[2], newVel };
            return returnVel;
        }
        public static float HandleAvgVelocity(ref float[] vel, float newVel)
        {
            vel = MyFunctions.AddTo3Velocities(vel, newVel);
            float addVel = 0;
            for (int i = 0; i < vel.Length; i++)
            {
                addVel += vel[i];
            }
            return addVel / 3;
        }
        public static ConfigurableJoint SetJointValues(JointValues _jointValues)
        {
            JointDrive _jointDrive = new JointDrive();
            _jointDrive.positionSpring = _jointValues.positionSpring;
            _jointDrive.positionDamper = _jointValues.positionDumper;
            _jointDrive.maximumForce = _jointValues.maximumForce;

            JointDrive _angularJointDrive = new JointDrive();
            _angularJointDrive.positionSpring = _jointValues.angularSpring;
            _angularJointDrive.positionDamper = _jointValues.angularDumper;
            _angularJointDrive.maximumForce = _jointValues.angularMaximumForce;

            _jointValues.joint.xDrive = _jointDrive;
            _jointValues.joint.yDrive = _jointDrive;
            _jointValues.joint.zDrive = _jointDrive;

            _jointValues.joint.slerpDrive = _angularJointDrive;
                return _jointValues.joint;
        }
        public static void CopyTransfomrValues(Transform _transformIn, ref Transform _transformOut)
        {
            //_transformOut = new GameObject().transform;
            _transformOut.position = _transformIn.position;
            _transformOut.rotation = _transformIn.rotation;
            _transformOut.localScale = _transformIn.localScale;
            Transform parent = _transformIn.parent ? _transformIn.parent : null;

            if (parent)
            {
                _transformOut.SetParent(_transformIn.parent);
            }
            else
            {
                _transformOut.SetParent(null);
            }
        }
        public static void SetConfigurableJointMotionType(ref ConfigurableJoint _joint, ConfigurableJointMotion _linearMotionType,
            ConfigurableJointMotion _angularMotionType)
        {
            _joint.xMotion = _linearMotionType;
            _joint.yMotion = _linearMotionType;
            _joint.zMotion = _linearMotionType;
            _joint.angularXMotion = _angularMotionType;
            _joint.angularYMotion = _angularMotionType;
            _joint.angularZMotion = _angularMotionType;
        }
    }
    
}
