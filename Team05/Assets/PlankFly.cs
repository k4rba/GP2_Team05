using UnityEngine;
using Random = UnityEngine.Random;

public class PlankFly : MonoBehaviour
{
    private void Awake() {
        GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * 6, ForceMode.VelocityChange);
        GetComponent<Rigidbody>().AddTorque(Random.onUnitSphere * 6, ForceMode.VelocityChange);
    }
}
