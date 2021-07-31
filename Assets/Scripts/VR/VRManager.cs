using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR.Base
{
    public class VRManager : MonoBehaviour
    {

        [SerializeField] protected LayerMask interactableLayerMask;
        [Header("---References---")]
        [SerializeField] VRController rightController;
        [SerializeField] VRController leftController;
        [SerializeField] Transform head;

        //[SerializeField] Hand dominantHand = Hand.Right;
        //[SerializeField] float handToAttachPointVelocity = 5f;
        [SerializeField] float maxItemHover = 5f;
        [Header("ray attach parameters")]
        [SerializeField] float maxHoverRayDist = 5f;
        [SerializeField] float forceAttachOnRayDist = 0.2f;
        [SerializeField] float speedToHandMul = 5.5f;
        [SerializeField] float ySpeedToHandAdd = 1.1f;
        [Header("---DefaultHandSettings---")]
        [SerializeField] AnimationCurve dumperSpeedToVelocity;
        [SerializeField] float positionSpring = 2000f;
        [SerializeField] float positionDumper = 30f;
        [SerializeField] float maximumForce = 200f;
        [SerializeField] AnimationCurve dumperAngularSpeedToAngularVelocity;
        [SerializeField] float angularSpring = 800f;
        [SerializeField] float angularDumper = 50f;
        [SerializeField] float angularMaximumForce = 30f;
        //[SerializeField] float armLenght = 0.8f;
        //[SerializeField] float armLenghtSlope = 50;
        [SerializeField] AnimationCurve handDistanceHapticAmount;
        [SerializeField] AnimationCurve onCollisionHaptic;

        [Header("Body Collider variables")]
        //[SerializeField] float headColliderRadious = 0.15f;
        [SerializeField] float bodyColliderRadious = 0.1f;
        [SerializeField] float bodyHeight = 0.9f;
        [SerializeField] float headRadious = 0.1f;

        [SerializeField] Transform[] teleportTransforms;
        //[SerializeField] float footColliderRadious = 0.1f;

        List<VRInteractableBase> grabbedInteractables = new List<VRInteractableBase>();
        //List<VRHandInteractor> handInteractors = new List<VRHandInteractor>();

        #region Accesors
        //public Hand DominantHand { get { return dominantHand; } set { dominantHand = value; } }
        //public float HandToAttachPointVelocity { get { return handToAttachPointVelocity; } }
        public List<VRInteractableBase> GrabbedInteractables { get { return grabbedInteractables; } }
        public LayerMask InteractableLayerMask { get { return interactableLayerMask; } }
        public VRController RightController { get { return rightController; } }
        public VRController LeftController { get { return leftController; } }
        //public List<VRHandInteractor> HandInteractors { get { return handInteractors; } }
        public Transform Head { get { return head; } }

        public float MaxItemsHover { get { return maxItemHover; } }
        public float MaxHoverRayDist {  get { return maxHoverRayDist; } }

        public float ForceAttachOnRayDist {  get { return forceAttachOnRayDist; } }
        public float SpeedToHandMul {  get { return speedToHandMul; } }
        public float SpeedToHandMax {  get { return ySpeedToHandAdd; } }


        //------------ Hand Settings---------------
        public AnimationCurve DumperSpeedToVelocity { get { return dumperSpeedToVelocity; } }
        public float PositionSpring { get { return positionSpring; } }
        public float PositionDumper { get { return positionDumper; } }
        public float MaximumForce { get { return maximumForce; } }
        public AnimationCurve DumperAngularSpeedToAngularVelocity { get { return dumperAngularSpeedToAngularVelocity; } }
        public float AngularSpring { get { return angularSpring; } }
        public float AngularDumper { get { return angularDumper; } }
        public float AngularMaximumForce { get { return angularMaximumForce; } }
        //public float ArmLenght {  get { return armLenght; } set { armLenght = value; } }
        //public float ArmLenghtSlope {  get { return armLenghtSlope; } }
        public AnimationCurve HandDistanceHapticAmount {  get { return handDistanceHapticAmount; } }
        public AnimationCurve OnCollisionHaptic { get { return onCollisionHaptic; } }


        //--------------body variables -------------
       // public float HeadColliderRadious { get { return headColliderRadious; } }
        public float BodyColliderRadious { get { return bodyColliderRadious; } }
        public float BodyHeight {  get { return bodyHeight; } }
        public float HeadRadious { get { return headRadious; } }
        //public float FootColliderRadious { get { return footColliderRadious; } }
        //public VRRig VRRig { get; set; }
        #endregion

        private void FixedUpdate()
        {
            VRInteractableBase grabbed = null;
            for (int i = 0; i < GrabbedInteractables.Count; i++)
            {
                if (grabbed != GrabbedInteractables[i])
                {
                    grabbed = GrabbedInteractables[i];
                    grabbed.OnFixedUpdate(Time.deltaTime);
                }
            }
        }
        private void Update()
        {
            VRInteractableBase grabbed = null;
            for (int i = 0; i < GrabbedInteractables.Count; i++)
            {
                if (grabbed != GrabbedInteractables[i])
                {
                    grabbed = GrabbedInteractables[i];
                    grabbed.OnUpdate(Time.deltaTime);
                }
            }
        }

        public void AddGrabbedInteractable(VRInteractableBase _interactable)
        {
            foreach(VRInteractableBase interactable in grabbedInteractables)
            {
                if(interactable == _interactable)
                {
                    return;
                }
            }
            grabbedInteractables.Add(_interactable);
        }
        public void RemoveGrabbedInteractable(VRInteractableBase _interactable)
        {
            if (GrabbedInteractables.Count == 0)
            {
                return;
            }
            foreach (VRInteractableBase interactable in grabbedInteractables)
            {
                if (interactable == _interactable)
                {
                    grabbedInteractables.Remove(_interactable);
                    return;
                }
            }
            
        }
        //public void AddHandInteractor(VRHandInteractor _handInteracor)
        //{
        //    handInteractors.Add(_handInteracor);
        //}


        //TODO: Function will not work when left hand have ovverided joint values
        //public void CheckInteractablesIfHaveLeftHandOnly()
        //{
        //    foreach(VRInteractableBase interactable in GrabbedInteractables)
        //    {
        //        foreach(VRHandInteractor interactor in interactable.HandInteractors)
        //        {
        //            if(interactor.GetHand != DominantHand)
        //            {
        //                interactor.HandJointRestoreDefaultValues();
        //            }
        //        }
        //    }
        //}
        //public bool IsItDominantHandOnThatInteractable(VRHandInteractor _interactor)
        //{
        //    if(GrabbedInteractables.Count < 2)
        //    {
        //        return true;
        //    }
        //    if(GrabbedInteractables[0].gameObject == GrabbedInteractables[1].gameObject)
        //    {
        //        if(_interactor.GetHand == DominantHand)
        //        {
        //            foreach (VRHandInteractor hand in HandInteractors)
        //            {
        //                if (hand.GetHand != DominantHand)
        //                {
        //                    hand.MakeHandJointWouble();
        //                }
        //            }
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    return true; //if holding different objects
        //}
        //public void AddExtraAttachTransformOnRig(VRHandInteractor _interactor, Transform _transform)
        //{
        //    if(_interactor.GetHand == Hand.Right)
        //    {
        //        VRRig.RHand.ExtraAttachTransOffsetRot = _transform.localRotation;
        //        VRRig.RHand.ExtraAttachTransOffsetPos = _transform.localPosition;
        //    }
        //    else
        //    {
        //        VRRig.LHand.ExtraAttachTransOffsetRot = _transform.localRotation;
        //        VRRig.LHand.ExtraAttachTransOffsetPos = new Vector3(-_transform.localPosition.x,
        //            _transform.localPosition.y, _transform.localPosition.z);
        //    }
        //}
        //public void RemoveExtraAttachTransformOnRig(VRHandInteractor _interactor)
        //{
        //    if (_interactor.GetHand == Hand.Right)
        //    {
        //        VRRig.RHand.ExtraAttachTransOffsetRot = Quaternion.identity;
        //        VRRig.RHand.ExtraAttachTransOffsetPos = Vector3.zero;
        //    }
        //    else
        //    {
        //        VRRig.LHand.ExtraAttachTransOffsetRot = Quaternion.identity;
        //        VRRig.LHand.ExtraAttachTransOffsetPos = Vector3.zero;
        //    }
        //}
    }
}
