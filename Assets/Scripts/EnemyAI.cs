using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    Animator anim;
    NavMeshAgent agent;

    public Vector3 targetPos;

    public float speed;

    private void Start() {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        if (Vector3.Distance(targetPos, transform.position) > agent.radius) {
            agent.SetDestination(targetPos);

            agent.isStopped = false;

            if (agent.remainingDistance > agent.radius) {
                anim.SetFloat("Speed", speed);
            } else {
                anim.SetFloat("Speed", 0f, 2f, 5f * Time.deltaTime);
            }
        } else {
            agent.isStopped = true;
        }

        agent.nextPosition = transform.position;
    }

    public void WalkTo(Vector3 pos) {
        targetPos = pos;
    }

    private void OnAnimatorMove() {
        Vector3 targetVelocity = anim.deltaPosition / Time.deltaTime;

        print(targetVelocity+" "+targetVelocity.magnitude);        

        if (agent.remainingDistance > agent.radius) {
            agent.speed = targetVelocity.magnitude;
        } else {
            agent.speed = 0f;
        }
    }
}
