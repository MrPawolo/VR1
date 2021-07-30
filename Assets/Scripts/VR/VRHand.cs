using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR.Base
{
    public class VRHand : MonoBehaviour
    {
        public VRManager VRManager { get; set; }
        public ConfigurableJoint HandJoint { get; set; }
    }
}
