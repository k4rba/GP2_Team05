using UnityEngine;

namespace Andreas.Scripts
{
    public class RopeSegment : MonoBehaviour
    {
        private void Awake()
        {
            var body = GetComponent<Rigidbody>();
            body.centerOfMass = Vector3.zero;
            body.inertiaTensor = Vector3.one;
        }
    }
}