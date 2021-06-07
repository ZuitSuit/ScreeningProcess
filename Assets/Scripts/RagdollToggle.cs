using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollToggle : MonoBehaviour {

    Rigidbody[] bodies;
    Animator anim;
    NavMeshAgent agent;

    void Start(){
        bodies = GetComponentsInChildren<Rigidbody>();

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        SetKinematic(true);
    }

    public void SetKinematic(bool newValue) {
        foreach (Rigidbody rb in bodies) {
            rb.isKinematic = newValue;
        }
        anim.enabled = newValue;
        agent.enabled = newValue;
    }
}
