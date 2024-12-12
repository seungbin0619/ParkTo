using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsObject : MonoBehaviour
{
    public Rigidbody RB { get; private set; }

    [field: SerializeField]
    public Vector3 Size { get; private set; } = Vector3.one;

    protected virtual void Awake() {
        RB = GetComponent<Rigidbody>();
    }

    protected virtual void OnCollisionEnter(Collision collision) {
        //Collision
    }
}
