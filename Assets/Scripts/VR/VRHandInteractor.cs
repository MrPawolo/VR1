using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VR.Base
{
    public class VRHandInteractor : MonoBehaviour
    {
        [SerializeField] float interiaTensor = 0.003f;
        public VRController Controller;
        public Animator handAnimator;
        [Range(0,1)] public float minHandColsedVal = 0.1f;

        public UnityEvent onHoverEnter;
        public UnityEvent onHoverExit;
        public UnityEvent onAttach;
        public UnityEvent onDetach;
        public UnityEvent onTriggerButtonTrue;
        public UnityEvent onTriggerButtonFalse;

        public UnityEvent onGripButtonTrue;
        public UnityEvent onGripButtonFalse;

        public UnityEvent onFirstButtonTrue;
        public UnityEvent onFirstButtonFalse;

        public UnityEvent onSecondButtonTrue;
        public UnityEvent onSecondButtonFalse;

        public UnityEvent onJoystickButtonTrue;
        public UnityEvent onJoystickButtonFasle;

        public VRManager VRManager { get; set; }
        public ConfigurableJoint HandJoint { get; set; }
        public HandDistanceHaptic HandDistanceHaptic { get; set; }
        public VRInteractableBase GrabInteractable {  get { return grabInteractable; } }
        public Collider[] handColliders;

        public float AvgVelocity { get; set; }
        float[] velocities = new float[3];
        protected ConfigurableJoint myJoint;
        protected VRInteractableBase grabInteractable;
        private VRInteractableBase tempInteractable;
        public List<VRInteractableBase> HoveredObjects = new List<VRInteractableBase>();
        Rigidbody myRb;
        IEnumerator attachingCorutine;

        readonly int GRIP_ID = Animator.StringToHash("Grip");
        public void Awake()
        {
            myRb = GetComponent<Rigidbody>();
            myRb.inertiaTensor = interiaTensor * Vector3.one;
            HandDistanceHaptic = GetComponent<HandDistanceHaptic>();
        }
        private void FixedUpdate()
        {
            if (!grabInteractable)
            {
                if (HoveredObjects.Count > 0)
                {
                    tempInteractable = Calculate(tempInteractable, HoveredObjects);
                }
            }
            AvgVelocity = MyFunctions.HandleAvgVelocity(ref velocities, myRb.velocity.magnitude);
            HandleAnimation();
        }
        void HandleAnimation()
        {
            if (!handAnimator) return;
            if (grabInteractable)
            {
                handAnimator.SetFloat(GRIP_ID, 1);
            }
            else
            {
                handAnimator.SetFloat(GRIP_ID, Mathf.Lerp(minHandColsedVal, 1f, Mathf.SmoothStep(0,1,Controller.GripVal)) );
            }
        }
        VRInteractableBase Calculate(VRInteractableBase _tempInteractable, List<VRInteractableBase> _hoveredObjects)
        {
            if (_hoveredObjects.Count == 0)
            {
                Controller.OnHoverExit();
                return null;
            }
            else
            {
                float weightReference = Mathf.Infinity;
                VRInteractableBase previousInteractable = _tempInteractable;
                VRInteractableBase hoveredInteractable = null;

                foreach (VRInteractableBase interactable in _hoveredObjects)
                {
                    if (!interactable.Hoverable || (interactable.Grabbed && !interactable.Hoverable))
                    {
                        break;
                    }

                    float calculated = interactable.HoverWeight * (interactable.transform.position - transform.position).magnitude;

                    if (calculated < weightReference)
                    {
                        weightReference = calculated;
                        hoveredInteractable = interactable;
                    }
                    else
                    {
                        if (interactable == previousInteractable)
                        {
                            interactable.OnHoverExit(this); //TODO: daje sygna� mimo �e nie by� wcze�nieje podswietlony
                            OnHoverExit(interactable);
                        }
                    }
                }
                _tempInteractable = hoveredInteractable;
                //_tempInteractable.GetAttachTransform(this);
                if (_tempInteractable != previousInteractable && _tempInteractable != null)
                {
                    Controller.OnHoverEnter();
                    _tempInteractable.OnHoverEnter(this);
                    OnHoverEnter(_tempInteractable);
                }
                return _tempInteractable;
            }
        }
        public void OnGripPressed()
        {
            onGripButtonTrue.Invoke();
            if (tempInteractable != null)
            {
                grabInteractable = tempInteractable;
                TryToAttach(grabInteractable);
            }
        }
        public void OnGripReleased()
        {
            onGripButtonFalse.Invoke();
            TryToDetach();
        }
        public void OnTriggerPressed()
        {

        }
        public void OnTriggerReleased()
        {

        }
        public virtual void OnAttach(VRInteractableBase _interactableBase)
        {
            onAttach?.Invoke();
        }
        public virtual void OnDetach(VRInteractableBase _interactableBase)
        {
            onDetach.Invoke();
        }
        public virtual void OnHoverEnter(VRInteractableBase _interactableBase)
        {
            onHoverEnter.Invoke();
        }
        public virtual void OnHoverExit(VRInteractableBase _interactableBase)
        {
            onHoverExit.Invoke();
        }
        public virtual void TryToAttach(VRInteractableBase _interactable)
        {
            if (_interactable != null && attachingCorutine == null)
            {
                tempInteractable?.OnHoverExit(this);

                grabInteractable.Grabbed = true;
                if (!grabInteractable.MultipleGrab)
                {
                    grabInteractable.Hoverable = false;
                }
                tempInteractable = null;
                attachingCorutine = Attach(_interactable);
                StartCoroutine(attachingCorutine);
                //Attach(_interactable);
                HoveredObjects.Clear();
            }
        }
        IEnumerator Attach(VRInteractableBase _grabInteractable) //TODO: change to corutine, to add hand position to attach point animation
        {
            _grabInteractable.OnAttachBegin(this);

            Transform handTransformBackup = this.transform;
            EnableAllColliders(false);

            /*if (attachTransform)
            {
                Vector3 startPos = this.transform.position;
                Quaternion startRot = transform.rotation;
                float distance = (startPos - attachTransform.position).magnitude;
                float time = 0;
                float incrementPerFrame = 1 / Mathf.Max((distance / VRManager.HandToAttachPointVelocity), 0.06f) * Time.fixedDeltaTime;

                while (time < 1)
                {
                    time += incrementPerFrame;
                    float t = Mathf.SmoothStep(0, 1, time);
                    transform.position = Vector3.Lerp(startPos, attachTransform.position, t);
                    transform.rotation = Quaternion.Lerp(startRot, attachTransform.rotation, t);
                    yield return new WaitForFixedUpdate();
                }

                transform.rotation = attachTransform.rotation;
                transform.position = attachTransform.position;
                //transform.rotation = attachTransform.rotation;
                //transform.position = attachTransform.position;

                //.GetComponentInChildren(typeof(Transform)) as Transform;

                if (attachTransform.childCount > 0)//this allow to set the animation hand attach transform
                {
                    Transform extraAT = attachTransform.GetChild(0);
                    Debug.Log(extraAT.localRotation.eulerAngles);
                    VRManager.AddExtraAttachTransformOnRig(this, extraAT);
                }
            }*/

            ConfigureJoint();
            VRManager.AddGrabbedInteractable(grabInteractable);

            _grabInteractable.AddHandInteractor(this); //TODO: Trzeba sprawdzi� czy to musi by� w tym miejscu, prawdopodobnie lepiej bedzie jak b�dzie dodana r�ka po animacji do��czania
            _grabInteractable.OnAttachEnd(this);
            Controller.OnAttach();
            onAttach?.Invoke();
            yield return null;
            //if (!VRManager.IsItDominantHandOnThatInteractable(this))//TODO: To powinno by� sprawdzane na obiekcie
            //{
            //    MakeHandJointWouble();
            //}
        }
        private void ConfigureJoint()
        {
            Rigidbody rb = grabInteractable.GetComponent<Rigidbody>();
            myJoint = this.gameObject.AddComponent<ConfigurableJoint>();
            myJoint.connectedBody = rb;

            myJoint.axis = new Vector3(0, 0, 0);
            myJoint.secondaryAxis = new Vector3(0, 1.5f, 1.5f);

            myJoint.angularXMotion = ConfigurableJointMotion.Locked;
            myJoint.angularYMotion = ConfigurableJointMotion.Locked;
            myJoint.angularZMotion = ConfigurableJointMotion.Locked;
            myJoint.xMotion = ConfigurableJointMotion.Locked;
            myJoint.yMotion = ConfigurableJointMotion.Locked;
            myJoint.zMotion = ConfigurableJointMotion.Locked;
        }
        [ContextMenu("ManualDetach")]
        public virtual void TryToDetach()
        {
            if (grabInteractable)
            {
                if (attachingCorutine != null)
                {
                    StopCoroutine(attachingCorutine);
                    attachingCorutine = null;
                }
                EnableAllColliders(true);
                StartCoroutine(ResetTriggerCollider());

                if (myJoint)
                {
                    Destroy(myJoint);
                }
                grabInteractable.RemoveHandInteractor(this);
                grabInteractable.OnDetach(this);
                VRManager.RemoveGrabbedInteractable(grabInteractable);

                Controller.OnDetach();
                onDetach?.Invoke();
                //VRManager.CheckInteractablesIfHaveLeftHandOnly();//TODO: To powinno by� sprawdzane na obiekcie
                if (!grabInteractable.MyRb.isKinematic)
                {
                    StartCoroutine(IgnoreCollisionForTime(grabInteractable.CollisionColliders));
                }
                grabInteractable.Grabbed = false;
                grabInteractable.Hoverable = true;
                grabInteractable = null;
            }
        }
        IEnumerator ResetTriggerCollider()//TODO: Temp solution can cause bug in the future its make to make socket interactor work properly
        {
            foreach (Collider collider in handColliders)
            {
                if (collider.isTrigger)
                {
                    collider.enabled = false;
                }
            }
            yield return new WaitForFixedUpdate();
            foreach (Collider collider in handColliders)
            {
                if (collider.isTrigger)
                {
                    collider.enabled = true;
                }
            }
        }

        IEnumerator IgnoreCollisionForTime(Collider[] colliders)
        {
            IgnoreCollision(colliders, true);
            yield return new WaitForSeconds(0.8f);
            IgnoreCollision(colliders, false);
        }

        private void IgnoreCollision(Collider[] colliders, bool state)
        {
            foreach (Collider collider in colliders)
            {
                foreach (Collider myCollider in handColliders)
                {
                    Physics.IgnoreCollision(myCollider, collider, state);
                }
            }
        }
        public virtual void OnTriggerEntered(Collider _other)
        {
            if (GrabInteractable)
            {
                return;
            }
            if (_other.gameObject.TryGetComponent<VRInteractableBase>(out VRInteractableBase interactable))
            {
                if (!interactable.Hoverable || (interactable.Grabbed && !interactable.Hoverable))
                {
                    return;
                }
                if (HoveredObjects.Count >= VRManager.MaxItemsHover)
                {
                    return;
                }
                else
                {
                    bool theSame = false;
                    foreach (VRInteractableBase temopInteractable in HoveredObjects) //check if there is no doubles
                    {
                        if (temopInteractable == interactable)
                        {
                            theSame = true;
                        }
                    }
                    if (!theSame)
                    {
                        HoveredObjects.Add(interactable);
                    }
                    else { return; }

                }
            }
        }
        public virtual void OnTriggerExited(Collider _other)
        {
            if (GrabInteractable)
            {
                return;
            }
            if (_other.gameObject.TryGetComponent<VRInteractableBase>(out VRInteractableBase _interactable))
            {
                _interactable.OnHoverExit(this);
                HoveredObjects.Remove(_interactable);
                tempInteractable = null;
            }
        }
        private void OnTriggerEnter(Collider _other)
        {
            if (((1 << _other.gameObject.layer) & VRManager.InteractableLayerMask) != 0)
            {
                OnTriggerEntered(_other);
            }
        }
        private void OnTriggerExit(Collider _other)
        {
            if (((1 << _other.gameObject.layer) & VRManager.InteractableLayerMask) != 0)
            {
                OnTriggerExited(_other);
            }
        }
        void EnableAllColliders(bool _state)
        {
            foreach (Collider collider in handColliders)
            {
                if (!collider.isTrigger)
                {
                    collider.enabled = _state;
                }
            }
        }
    }
}
