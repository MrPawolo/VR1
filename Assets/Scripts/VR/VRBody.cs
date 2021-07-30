using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR.Base {
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class VRBody : MonoBehaviour
    {
        [SerializeField] VRManager vRManager;
        [SerializeField] Transform IO;
        [SerializeField] VRHand rightHand;
        [SerializeField] VRHand leftHand;

        ConfigurableJoint rightHandJoint;
        ConfigurableJoint leftHandJoint;

        CapsuleCollider bodyCollider;
        SphereCollider headCollider;

        Rigidbody myRb;


        #region Accesors
        #endregion
        void Awake()
        {
            myRb = GetComponent<Rigidbody>();
            //bodyCollider = GetComponent<CapsuleCollider>();
            //bodyCollider.radius = vRManager.BodyColliderRadious;
            headCollider = GetComponent<SphereCollider>();
            headCollider.radius = vRManager.HeadRadious;
            rightHandJoint = SetHand(rightHand);
            leftHandJoint = SetHand(leftHand);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            SetHandPositionAndRotation(rightHandJoint, vRManager.RightController);
            SetHandPositionAndRotation(leftHandJoint, vRManager.LeftController);
            HandJoitValues(rightHandJoint, vRManager.RightController);
            HandJoitValues(leftHandJoint, vRManager.LeftController);
            CenterIO();
            CenterColliders();
        }
        void HandJoitValues(ConfigurableJoint _handJoint, VRController _controller)
        {
            JointValues jointValues = new JointValues();
            jointValues.joint = _handJoint;

            float positionSpring = vRManager.PositionSpring;
            float positionDumper = vRManager.PositionDumper;
            float maximumForce = vRManager.MaximumForce;

            float angularSpring = vRManager.AngularSpring;
            float angularDumper = vRManager.AngularDumper;
            float angularMaximumForce = vRManager.AngularMaximumForce;

            //if (grabInteractable?.JointOvveride)
            //{
            //    positionSpring = grabInteractable.PositionSpringOverride;
            //    positionDumper = grabInteractable.PositionDumperOverride;
            //    maximumForce = grabInteractable.MaximumForceOverride;

            //    angularSpring = grabInteractable.AngularSpringOverride;
            //    angularDumper = grabInteractable.AngularDumperOverride;
            //    angularMaximumForce = grabInteractable.AngularMaximumForceOverride;
            //}

            float positionDumperVal = vRManager.DumperSpeedToVelocity.Evaluate(_controller.Velocity.magnitude) * positionDumper;
            float angularDumperVal = vRManager.DumperAngularSpeedToAngularVelocity.Evaluate(_controller.AngularVelocity.magnitude) * angularDumper;


            jointValues.positionSpring = positionSpring;// * armLenghtModifier;
            jointValues.positionDumper = positionDumperVal;// * Mathf.Min(armLenghtModifier,1);
            jointValues.maximumForce = maximumForce;// * armLenghtModifier;

            jointValues.angularSpring = angularSpring;// * woubleModifier;
            jointValues.angularDumper = angularDumperVal;// * woubleModifier;
            jointValues.angularMaximumForce = angularMaximumForce;// * woubleModifier;

            MyFunctions.SetJointValues(jointValues);
        }
        ConfigurableJoint SetHand(VRHand hand)
        {
            ConfigurableJoint joint = this.gameObject.AddComponent<ConfigurableJoint>();
            //Vector3 conectedAnchor;
            VRHand handInteractor = hand.GetComponent<VRHand>();

            handInteractor.VRManager = vRManager;
            handInteractor.HandJoint = joint;
            //VRManager.AddHandInteractor(handInteractor);

            //configure joint
            joint.connectedBody = hand.GetComponent<Rigidbody>();
            joint.rotationDriveMode = RotationDriveMode.Slerp;
            joint.configuredInWorldSpace = false;// true;
            joint.projectionMode = JointProjectionMode.PositionAndRotation;
            joint.massScale = 1f;
            joint.connectedMassScale = 1f;
            joint.autoConfigureConnectedAnchor = false;

            MyFunctions.SetConfigurableJointMotionType(ref joint, ConfigurableJointMotion.Free, ConfigurableJointMotion.Free);


            //prepare joint Values
            JointValues jointValues = new JointValues();
            jointValues.positionSpring = vRManager.PositionSpring;
            jointValues.positionDumper = vRManager.PositionDumper;
            jointValues.maximumForce = vRManager.MaximumForce;
            jointValues.angularSpring = vRManager.AngularSpring;
            jointValues.angularDumper = vRManager.AngularDumper;
            jointValues.angularMaximumForce = vRManager.AngularMaximumForce;
            jointValues.joint = joint;
            //Set joint values
            MyFunctions.SetJointValues(jointValues);

            return joint;
        }
        void SetHandPositionAndRotation(ConfigurableJoint _joint, VRController _controller)
        {
            _joint.targetPosition = transform.InverseTransformPoint(_controller.GetAttachTransform.position);
            _joint.targetRotation = _controller.GetAttachTransform.rotation;
        }
        void CenterIO() //Camera and controller's parent
        {
            IO.transform.position = transform.position;
        }
        void CenterColliders()
        {
            Vector3 headPos = transform.InverseTransformPoint(vRManager.Head.position);
            //CenterBody(headPos);
            Centerhead(headPos);
        }
        void CenterBody(Vector3 _headPos)
        {
            Vector3 headPos = _headPos;

            float headHeight = Mathf.Clamp(headPos.y, 0.5f, 2f);
            bodyCollider.height = vRManager.BodyHeight;

            Vector3 newCenter = Vector3.zero;

            newCenter.y = headPos.y;
            newCenter.y -= bodyCollider.height / 2;

            newCenter.x = headPos.x;
            newCenter.z = headPos.z;
            bodyCollider.center = newCenter;
        }
        void Centerhead(Vector3 _headPos)
        {
            headCollider.center = _headPos;
        }
        private Vector2 SpinePos()
        {
            Vector3 right = Vector3.ProjectOnPlane(vRManager.Head.right, Vector3.up).normalized;
            Vector3 back = Vector3.Cross(right, Vector3.up);
            Vector3 spinePos = transform.InverseTransformDirection(back) * -0.2f;
            spinePos = transform.InverseTransformPoint(vRManager.Head.position) + spinePos;

            return new Vector2(spinePos.x, spinePos.z);
        }

    } 
}
