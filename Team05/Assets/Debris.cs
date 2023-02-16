using UnityEngine;
using Random = UnityEngine.Random;

public class Debris : MonoBehaviour
{
    [SerializeField] private float Force = 5f;
    [SerializeField] private float Torque = 5f;
    private void Awake()
    {
        var body = GetComponent<Rigidbody>();
        body.AddForce(Random.onUnitSphere * Force, ForceMode.VelocityChange);
        body.AddTorque(Random.onUnitSphere * Torque, ForceMode.VelocityChange);
    }
}
