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
        [SerializeField] VRHandInteractor rightHand;
        [SerializeField] VRHandInteractor leftHand;



        [Header("---Turn Options---")]
        [SerializeField] VRController turnController;
        [SerializeField] TurnType turnType = TurnType.Snap;
        [SerializeField] float turnAngle = 45;
        [Tooltip("Deg per sec")]
        [SerializeField] float turnSpeed = 270;
        [SerializeField] float turnThresholdVal = 0.7f;

        bool turned = false;
        bool turning = false;
        float desiredTurn = 0f;
        float actTurn = 0f;

        Vector3 virtualPos = Vector3.zero;

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
            TurnProcess();
            
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

            VRInteractableBase grabInteractable = _controller.HandInteractor.GrabInteractable;
            if (_controller.HandInteractor.GrabInteractable?.JointOvveride)
            {
                positionSpring = grabInteractable.PositionSpringOverride;
                positionDumper = grabInteractable.PositionDumperOverride;
                maximumForce = grabInteractable.MaximumForceOverride;

                angularSpring = grabInteractable.AngularSpringOverride;
                angularDumper = grabInteractable.AngularDumperOverride;
                angularMaximumForce = grabInteractable.AngularMaximumForceOverride;
            }

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
        ConfigurableJoint SetHand(VRHandInteractor hand)
        {
            ConfigurableJoint joint = this.gameObject.AddComponent<ConfigurableJoint>();
            //Vector3 conectedAnchor;
            VRHandInteractor handInteractor = hand.GetComponent<VRHandInteractor>();

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
        //void CenterBody(Vector3 _headPos)
        //{
        //    Vector3 headPos = _headPos;

        //    float headHeight = Mathf.Clamp(headPos.y, 0.5f, 2f);
        //    bodyCollider.height = vRManager.BodyHeight;

        //    Vector3 newCenter = Vector3.zero;

        //    newCenter.y = headPos.y;
        //    newCenter.y -= bodyCollider.height / 2;

        //    newCenter.x = headPos.x;
        //    newCenter.z = headPos.z;
        //    bodyCollider.center = newCenter;
        //}
        void Centerhead(Vector3 _headPos)
        {
            headCollider.center = _headPos;
        }

        private void TurnProcess()
        {
            float joyStickXVal = turnController.JoyStickVal.x;
            if (Mathf.Abs(joyStickXVal) > turnThresholdVal )
            {
                if (!turned && turnType == TurnType.Snap)
                {
                    if (joyStickXVal > 0)
                    {
                        if (!turning)
                        {
                            turning = true;
                            desiredTurn = turnAngle;
                        }
                    }
                    else
                    {
                        if (!turning)
                        {
                            turning = true;
                            desiredTurn = -turnAngle;
                        }
                    }
                    turned = true;
                }
                else if(turnType == TurnType.Smooth)
                {
                    SmoothTurn(joyStickXVal);
                }
            }
            else if (Mathf.Abs(joyStickXVal) < turnThresholdVal)
            {
                turned = false;
            }

            if (turning && turnType == TurnType.Snap)
            {
                SmoothSnapTurn();
            }
        }
        void SmoothTurn(float _direction)
        {
            if (_direction > 0)
            {
                _direction = 1;
            }
            else
            {
                _direction = -1;
            }
            Vector3 prevPos = IO.localPosition;
            float _turnAmount = turnSpeed * Time.deltaTime * _direction;
            IO.transform.RotateAround(vRManager.Head.transform.position, Vector3.up, _turnAmount);
            virtualPos = virtualPos + (IO.localPosition - prevPos);
        }
        private void SmoothSnapTurn()
        {
            Vector3 prevPos = IO.localPosition;
            float _turnAmount = (desiredTurn / Mathf.Abs(desiredTurn)) * turnSpeed * Time.fixedDeltaTime;
            IO.transform.RotateAround(vRManager.Head.transform.position, Vector3.up, _turnAmount);
            virtualPos = virtualPos + (IO.localPosition - prevPos);
            actTurn += _turnAmount;

            if (Mathf.Abs(actTurn) >= Mathf.Abs(desiredTurn))
            {
                turning = false;
                actTurn = 0;
                desiredTurn = 0;
            }
        }
    } 
}
