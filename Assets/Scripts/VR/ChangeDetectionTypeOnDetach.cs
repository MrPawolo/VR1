using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR.Base
{
    [RequireComponent(typeof(VRInteractableBase))]
    public class ChangeDetectionTypeOnDetach : MonoBehaviour
    {
        [SerializeField] float time = 2f;
        VRInteractableBase interactable;
        public bool isEnabled = true;
        [SerializeField] CollisionDetectionMode defaultDetectionMode = CollisionDetectionMode.Discrete;
        [SerializeField] CollisionDetectionMode attachedDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        [SerializeField] CollisionDetectionMode detachedDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        IEnumerator changingTypeCorutine;
        private void OnEnable()
        {
            if (!interactable)
            {
                interactable = GetComponent<VRInteractableBase>();
            }
            interactable.onDetach.AddListener(ChangeDetectionType);
            interactable.onAttachEnd.AddListener(ChangeDetectionTypeOnAttach);
        }
        private void OnDisable()
        {
            interactable.onDetach.RemoveListener(ChangeDetectionType);
            interactable.onAttachEnd.RemoveListener(ChangeDetectionTypeOnAttach);
        }
        private void ChangeDetectionType()
        {
            if (isEnabled)
            {
                if (changingTypeCorutine != null)
                {
                    StopCoroutine(changingTypeCorutine);
                }
                changingTypeCorutine = WaitAndChange();
                ChangeDetectionTypeTo(detachedDetectionMode);
                StartCoroutine(changingTypeCorutine);
            }
        }
        void ChangeDetectionTypeOnAttach()
        {
            if (isEnabled)
            {
                if(changingTypeCorutine != null)
                {
                    StopCoroutine(changingTypeCorutine);
                    changingTypeCorutine = null;
                }
                ChangeDetectionTypeTo(attachedDetectionMode);
            }
        }
        IEnumerator WaitAndChange()
        {
            yield return new WaitForSeconds(time);
            ChangeDetectionTypeTo(defaultDetectionMode);
        }
        void ChangeDetectionTypeTo(CollisionDetectionMode _collisionDetectionMode)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb)
            {
                rb.collisionDetectionMode = _collisionDetectionMode;
            }
        }

    }
}
