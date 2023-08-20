using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private Rigidbody[] rigidbodies;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        DeactiveRagdoll();
    }

    private void DeactiveRagdoll()
    {
        foreach(var rigidBody in rigidbodies) {
            rigidBody.isKinematic = true;
        }
        anim.enabled = true;
    }

    public void ActivateRagdoll()
    {
        foreach (var rigidBody in rigidbodies) {
            rigidBody.isKinematic = false;
        }
        anim.enabled = false;
    }
}
