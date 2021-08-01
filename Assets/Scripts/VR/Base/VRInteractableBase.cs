using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VR.ObjectDefinitions;

namespace VR.Base
{
    public class VRInteractableBase : MonoBehaviour
    {
        [SerializeField] float hoverWeight = 1;
        [SerializeField] protected InteractableJointOvverideValSO jointOvveride;
        [SerializeField] bool multipleGrab = true;
        [SerializeField] List<Collider> hoverColliders = new List<Collider>();
        [SerializeField] Collider[] collisionColliders;



        protected bool hoverable = true;
        List<VRHandInteractor> vRHandInteractors = new List<VRHandInteractor>();

        #region Accesors
        public bool MultipleGrab { get { return multipleGrab; } set { multipleGrab = value; } } 
        public float HoverWeight { get { return hoverWeight; } }
        public bool Hoverable { get { return hoverable; } set { hoverable = value; } }
        public bool Grabbed { get; set; }
        public List<VRHandInteractor> VRHandInteractors { get { return vRHandInteractors; } set { vRHandInteractors = value; } }
        public InteractableJointOvverideValSO JointOvveride { get { return jointOvveride; } }

        public virtual float PositionSpringOverride { get { return jointOvveride.PositionSpring; } }
        public virtual float PositionDumperOverride { get { return jointOvveride.PositionDumper; } }
        public virtual float MaximumForceOverride { get { return jointOvveride.MaximumForce; } }
        public virtual float AngularSpringOverride { get { return jointOvveride.AngularSpring; } }
        public virtual float AngularDumperOverride { get { return jointOvveride.AngularDumper; } }
        public virtual float AngularMaximumForceOverride { get { return jointOvveride.AngularMaximumForce; } }
        public Rigidbody MyRb { get { return myRb; } }
        public VRManager VRManager { get; set; }
        public Collider[] CollisionColliders { get { return collisionColliders; } }

        OnCollisionHit onCollision;
        Rigidbody myRb;
        #endregion

        #region UnityEvents
        public UnityEvent<VRInteractableBase> onAttachBegin;
        public UnityEvent onAttachEnd;
        public UnityEvent onDetach;
        public UnityEvent onHoverEnter;
        public UnityEvent onHoverExit;
        public UnityEvent onTriggerTrue;
        public UnityEvent onTriggerFalse;
        #endregion

        public void OnValidate()
        {
            if (onCollision == null)
            {
                onCollision = GetComponent<OnCollisionHit>();
            }
        }
        public void Start()
        {
            if (onCollision == null)
            {
                onCollision = GetComponent<OnCollisionHit>();
            }
        }
        public void OnUpdate(float _deltaTime)
        {

        }
        public void OnFixedUpdate(float _deltaTime)
        {
            DistanceHaptic();
        }
        public void OnDisable()
        {
            if (VRHandInteractors.Count > 0)
            {
                foreach (VRHandInteractor interactor in VRHandInteractors)
                {
                    interactor.TryToDetach();
                }
            }
        }
        public void AddHandInteractor(VRHandInteractor interactor)
        {
            if (!VRHandInteractors.Contains(interactor))
            {
                VRHandInteractors.Add(interactor);
            }
        }
        public void RemoveHandInteractor(VRHandInteractor interactor)
        {
            if (VRHandInteractors.Contains(interactor))
            {
                VRHandInteractors.Remove(interactor);
            }
        }
        public void RemoveAllHandInteractors()
        {
            VRHandInteractors.Clear();
        }
        public virtual void OnHoverEnter(VRHandInteractor handInteractor)
        {
            //GetAttachTransform(handInteractor);
            onHoverEnter?.Invoke();
        }
        public virtual void OnHoverExit(VRHandInteractor handInteractor)
        {
            onHoverExit?.Invoke();
        }
        public virtual void OnAttachBegin(VRHandInteractor handInteractor)
        {
            onAttachBegin?.Invoke(this);
        }
        public virtual void OnAttachEnd(VRHandInteractor handInteractor)
        {
            onAttachEnd?.Invoke();
            VRManager = handInteractor.VRManager;
            if(myRb == null)
            {
                myRb = GetComponent<Rigidbody>();
            }
        }
        public virtual void OnDetach(VRHandInteractor handInteractor)
        {
            onDetach?.Invoke();
        }

        public virtual void OnTriggerTrue()
        {
            onTriggerTrue?.Invoke();
        }
        public virtual void OnTriggerFalse()
        {
            onTriggerFalse?.Invoke();
        }

        public virtual void EnableAllHovers(bool _state)
        {
            foreach (Collider _collider in hoverColliders)
            {
                _collider.enabled = _state;
            }
        }
        public void RemoveVRManager()
        {
            VRManager = null;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (onCollision != null)
            {
                onCollision.OnCollision(MyFunctions.SmoothStep(0.5f,8, collision.relativeVelocity.magnitude), collision);
            }
            if (VRHandInteractors.Count == 0) { return; }
            if (myRb.isKinematic) {return; }
            foreach (VRHandInteractor interactor in VRHandInteractors)
            {
                float hitVelSqr = collision.relativeVelocity.magnitude;
                float haptic = VRManager.OnCollisionHaptic.Evaluate(hitVelSqr);
                interactor.Controller.SendHapticImpulse(0.1f, haptic);
                
            }
        }
        private void OnCollisionStay(Collision collision)
        {
            if (VRHandInteractors.Count == 0) { return; }
            if (myRb.isKinematic) { return; }
            foreach (VRHandInteractor interactor in VRHandInteractors)
            {
                float distSqr = (interactor.Controller.transform.position - interactor.transform.position).magnitude;
                float haptic = VRManager.HandDistanceHapticAmount.Evaluate(distSqr);
                interactor.Controller.SendHapticImpulse(0.1f, haptic);
            }
        }
        void DistanceHaptic()
        {
            if (VRHandInteractors.Count == 0) { return; }
            if (!myRb.isKinematic) { return; }
            foreach (VRHandInteractor interactor in VRHandInteractors)
            {
                float distSqr = (interactor.Controller.transform.position - interactor.transform.position).magnitude;
                float haptic = VRManager.HandDistanceHapticAmount.Evaluate(distSqr);
                interactor.Controller.SendHapticImpulse(0.1f, haptic);
            }
        }
    }
}
