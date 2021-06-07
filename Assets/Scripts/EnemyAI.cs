using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using GD.MinMaxSlider;

public class EnemyAI : MonoBehaviour {

    Animator anim;
    NavMeshAgent agent;

    public Vector3 targetPos;

    public float speed;

    [SerializeField]
    private BehaviorTree tree;

    Transform head;

    Transform player;

    [Header("AI General Settings")]
    public float sightRange = 10f;
    [Range(0, 360)]
    public float fovRange = 60f;

    [Header("AI Patrol Settings")]
    public PatrolLoopType patrolLoopType;

    [MinMaxSlider(0, 60)]
    public Vector2 minMaxIdleTime;

    float idleTime;

    int patrolPathIndex;

    public enum PatrolLoopType {
        WalkBack,
        GoToFirst
    }

    public Transform patrolPathParent;

    bool isDead = false;

    private void Start() {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        head = anim.GetBoneTransform(HumanBodyBones.Head);

        player = GameObject.FindGameObjectWithTag("Player").transform;

        tree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .Sequence()
                    .Condition("Sees player", () => SeesObject(player))
                    .Condition("Player in range", () => InRange(player.position, transform.position, sightRange))
                    .Condition("Player in FOV", () => InFov(player.position, fovRange))
                        .Do("Shoot player", () => {


                            return TaskStatus.Success;
                        })
                        .End()

                .Sequence()
                    .Do("Walk patrol path", () => {
                        if (idleTime > 0f) {
                            idleTime -= Time.deltaTime;
                            SetWalkingSpeed(0f);

                            return TaskStatus.Success;
                        }

                        Transform nextPatrolPoint = GetNextPatrolPoint();

                        if (Vector3.Distance(transform.position, nextPatrolPoint.position) > 1f) {
                            agent.SetDestination(nextPatrolPoint.position);
                            SetWalkingSpeed(1f);
                        } else {
                            IncrementPatrolPointIndex();
                            idleTime = Random.Range(minMaxIdleTime.x, minMaxIdleTime.y);
                        }


                        return TaskStatus.Success;
                    })

                    .End()
                    .Build();
    }

    void IncrementPatrolPointIndex() {
        patrolPathIndex++;

        if (patrolPathIndex >= patrolPathParent.childCount) {
            if (patrolLoopType == PatrolLoopType.GoToFirst) {
                patrolPathIndex %= patrolPathParent.childCount;
            }
        }
    }

    Transform GetNextPatrolPoint() {
        return patrolPathParent.GetChild(patrolPathIndex);
    }

    private void OnDrawGizmos() {
        if (patrolPathParent.childCount < 2) return;

        for (int i = 0; i < patrolPathParent.childCount-1; i++) {
            Gizmos.color = Color.Lerp(Color.red, Color.green, (float) i / (float) patrolPathParent.childCount);

            Transform thisPath = patrolPathParent.GetChild(i);
            Transform nextPath = patrolPathParent.GetChild(i+1);

            Gizmos.DrawLine(thisPath.position, nextPath.position);
        }

        if (patrolLoopType == PatrolLoopType.GoToFirst) {
            Gizmos.DrawLine(patrolPathParent.GetChild(0).position, patrolPathParent.GetChild(patrolPathParent.childCount - 1).position);
        }
    }

    private void Update() {
        if (!isDead) {
            tree.Tick();

            agent.nextPosition = transform.position;
        }
    }

    public void SetWalkingSpeed(float speed) {
        anim.SetFloat("Speed", speed, 2f, 5f * Time.deltaTime);
    }

    public void WalkTo(Vector3 pos) {
        targetPos = pos;
    }

    private void OnAnimatorMove() {
        Vector3 targetVelocity = anim.deltaPosition / Time.deltaTime;

        if (agent.remainingDistance > agent.radius) {
            agent.speed = targetVelocity.magnitude;
        } else {
            agent.speed = 0f;
        }
    }


    public bool InRange(Vector3 a, Vector3 b, float range) {
        return Vector3.Distance(a, b) <= range;
    }

    public bool SeesObject(Transform obj) {
        if (Physics.Raycast(head.position, (obj.position - head.position), out RaycastHit hit)) {
            if (hit.transform == obj) return true;
        }
        return false;
    }

    public bool InFov(Vector3 pos, float fovInDeg) {
        return Vector3.Angle(head.forward, pos - head.position) <= fovInDeg;
    }

    public void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Bullet")) {
            GetComponent<RagdollToggle>().SetKinematic(false);
            isDead = true;
        }
    }
}
