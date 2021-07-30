using UnityEngine;

namespace VR.ObjectDefinitions
{
    [CreateAssetMenu(fileName = "InteractableJointOvverideVal", menuName = "VR/InteractableJointOvverideVal")]
    public class InteractableJointOvverideValSO : ScriptableObject
    {
        [SerializeField] float positionSpring;
        [SerializeField] float positionDumper;
        [SerializeField] float maximumForce;
        [SerializeField] float angularSpring;
        [SerializeField] float angularDumper;
        [SerializeField] float maximumAngularForce;

        public float PositionSpring { get { return positionSpring; } }
        public float PositionDumper { get { return positionDumper; } }
        public float MaximumForce { get { return maximumForce; } }
        public float AngularSpring { get { return angularSpring; } }
        public float AngularDumper { get { return angularDumper; } }
        public float AngularMaximumForce { get { return maximumAngularForce; } }
    }
}
