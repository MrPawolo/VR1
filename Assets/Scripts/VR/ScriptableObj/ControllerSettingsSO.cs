using UnityEngine;

namespace VR.ObjectDefinitions
{
    [CreateAssetMenu(fileName = "ControllerSettings", menuName = "VR/ControllerSettings")]
    public class ControllerSettingsSO : ScriptableObject
    {
        [SerializeField] bool hapticOnHoverEnter = true;
        [SerializeField] float hoverEnterHapticAmount = 0.2f, hoverEnterHapticDuration = 0.2f;
        [SerializeField] bool hapticOnHoverExit = false;
        [SerializeField] float hoverExitHapticAmount = 0.2f, hoverExitHapticDuration = 0.2f;
        [SerializeField] bool hapticOnAttach = true;
        [SerializeField] float attachHapticAmount = 0.5f, attachHapticDuration = 0.2f;
        [SerializeField] bool hapticOnDetach = true;
        [SerializeField] float detachHapticAmount = 0.3f, detachHapticDuration = 0.2f;
        [Space(30)]
        [SerializeField] float maxDistFromHead = 1;

        public bool HapticOnHoverEnter { get { return hapticOnHoverEnter; } }
        public float HoverEnterHapticAmount {  get { return hoverEnterHapticAmount; } }
        public float HoverEnterHapticDuration {  get { return hoverEnterHapticDuration; } }
        public bool HapticOnHoverExit { get { return hapticOnHoverExit; } }
        public float HoverExitHapticAmount {  get { return hoverExitHapticAmount; } }
        public float HoverExitHapticDuration {  get { return hoverExitHapticDuration; } }
        public bool HapticOnAttach { get { return hapticOnAttach; } }
        public float AttachHapticAmount { get { return attachHapticAmount; } }
        public float AttachHapticDuration { get { return attachHapticDuration; } }
        public bool HapticOnDetach { get { return hapticOnDetach; } }
        public float DetachHapticAmount { get { return detachHapticAmount; } }
        public float DetachHapticDuration { get { return detachHapticDuration; } }
    }
}
