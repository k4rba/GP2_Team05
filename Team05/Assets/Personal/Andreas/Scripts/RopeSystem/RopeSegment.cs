using UnityEngine;

namespace Andreas.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class RopeSegment : MonoBehaviour
    {
    }

    [RequireComponent(typeof(HingeJoint))]
    public class RopeHinge : MonoBehaviour
    {
    }
}