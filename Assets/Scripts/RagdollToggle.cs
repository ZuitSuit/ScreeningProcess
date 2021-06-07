using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollToggle : MonoBehaviour {

    Rigidbody[] bodies;
    Animator anim;
    NavMeshAgent agent;
    Collider collider;

    void Start(){
        bodies = GetComponentsInChildren<Rigidbody>();

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<Collider>();

        SetKinematic(true);
    }

    public void SetKinematic(bool newValue) {
        foreach (Rigidbody rb in bodies) {
            rb.isKinematic = newValue;
            rb.GetComponent<Collider>().enabled = !newValue;
        }
        anim.enabled = newValue;
        agent.enabled = newValue;
        collider.enabled = newValue;
    }
}
