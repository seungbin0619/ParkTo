using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsObject : MonoBehaviour
{
    public Rigidbody Rb { get; private set; }

    [field: SerializeField]
    public Vector3 Size { get; private set; } = Vector3.one;

    void Awake() {
        Rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnCollisionEnter(Collision collision) {
        //Collision
    }
}
