using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using GD.MinMaxSlider;
using UnityEngine.Animations.Rigging;
using Lean.Pool;

public class EnemyAI : MonoBehaviour {

    Animator anim;
    NavMeshAgent agent;

    public float speed;

    [SerializeField]
    private BehaviorTree tree;

    public Transform head;

    Transform player;

    [Header("AI General Settings")]
    public float sightRange = 10f;
    [Range(0, 360)]
    public float fovRange = 60f;
    [Range(0f,1f)]
    public float inaccuracy;

    public float reactionTime = 0.7f;

    [Header("AI Patrol Settings")]
    public PatrolLoopType patrolLoopType;

    [MinMaxSlider(0, 60)]
    public Vector2 minMaxIdleTime;

    float idleTime;

    int patrolPathIndex;
    bool incrementPathIndex;

    public Transform gunBarrel;
    public RigBuilder rigBuilder;
    public Transform aimPoint;

    public enum PatrolLoopType {
        WalkBack,
        GoToFirst
    }

    public Transform patrolPathParent;

    bool isDead = false;

    [Header("Shooting")]
    public AudioSource shootSound;
    public AudioClip[] groundImpactSounds;
    public AudioClip[] fleshImpactSounds;

    public GameObject impactPrefab;

    public enum CurrentState {
        Patrol,
        Suspicious,
        Alert,
        Vanish
    }

    public CurrentState currentState;

    int suspicionLevel = 0;
    float suspicionTimer = 5f;
    float alertTimer = 0f;
    float vanishCounter = 0f;

    float reloadTime;

    float seeingPlayerTimer = 0f;

    private void Start() {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        aimPoint.name = "Aim Point " + Random.Range(0, 1000);
        aimPoint.transform.SetParent(null);

        /*tree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .Sequence()
                    .Condition("Sees player", () => FullySeesPlayer())
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
                    .Build();*/
    }

    void IncrementPatrolPointIndex() {
        if (incrementPathIndex) {
            patrolPathIndex++;
        } else {
            patrolPathIndex--;
            if (patrolPathIndex < 0) {
                patrolPathIndex = 0;
                incrementPathIndex = true;
            }
        }

        if (patrolPathIndex >= patrolPathParent.childCount) {
            if (patrolLoopType == PatrolLoopType.GoToFirst) {
                patrolPathIndex %= patrolPathParent.childCount;
            } else if (patrolLoopType == PatrolLoopType.WalkBack) {
                patrolPathIndex = patrolPathParent.childCount - 1;
                incrementPathIndex = false;
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

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(gunBarrel.position, gunBarrel.forward);

        Gizmos.DrawRay(head.position, head.forward);
    }

    private void Update() {
        if (!isDead) {
            //tree.Tick();

            agent.nextPosition = transform.position;

            reloadTime -= Time.deltaTime;

            if (agent.remainingDistance < agent.radius) {
                SetWalkingSpeed(0f);
            }

            if (rigBuilder.enabled && Vector3.Angle(head.forward, aimPoint.position - head.position) > 45f) {
                rigBuilder.enabled = false;
            }

            if (FullySeesPlayer() && !PlayerController.isDead) {
                aimPoint.position = Vector3.Lerp(aimPoint.position, player.position, 5f * Time.deltaTime);
                SetRigWeight(true);
                agent.SetDestination(player.position);

                if (Vector3.Dot(head.forward, ( player.position - head.position ).normalized) > 0.95f) {
                    seeingPlayerTimer += Time.deltaTime;
                    if (seeingPlayerTimer > reactionTime) {
                        ShootPlayer();
                    }
                } else {
                    seeingPlayerTimer = 0f;
                }
            }

            switch (currentState) {
                case CurrentState.Patrol:
                    SetRigWeight(false);

                    if (idleTime > 0f) {
                        idleTime -= Time.deltaTime;
                        SetWalkingSpeed(0f);
                    }

                    Transform nextPatrolPoint = GetNextPatrolPoint();

                    if (Vector3.Distance(transform.position, nextPatrolPoint.position) > 1f) {
                        agent.SetDestination(nextPatrolPoint.position);
                        SetWalkingSpeed(1f);
                    } else {
                        IncrementPatrolPointIndex();
                        idleTime = Random.Range(minMaxIdleTime.x, minMaxIdleTime.y);
                    }

                    if (FullySeesPlayer()) {
                        currentState = CurrentState.Suspicious;
                        suspicionTimer = 15f;

                        if (TouchingPlayer()) {
                            suspicionLevel = 3;
                            currentState = CurrentState.Alert;
                        }
                    }

                    break;
                case CurrentState.Suspicious:
                    if (suspicionLevel == 3) {
                        alertTimer = 15f;
                        currentState = CurrentState.Alert;
                    } else {
                        if (TouchingPlayer()) {
                            suspicionLevel = 3;
                            alertTimer = 15f;
                            currentState = CurrentState.Alert;
                        } else {
                            if (FullySeesPlayer()) {
                                suspicionTimer += 1f * Time.deltaTime;
                                if (suspicionTimer > 10f) {
                                    suspicionLevel = 3;
                                    alertTimer = 10f;
                                    currentState = CurrentState.Alert;
                                }
                            } else {
                                if (suspicionTimer < 0f) {
                                    suspicionLevel += 1;
                                    currentState = CurrentState.Patrol;
                                } else {
                                    suspicionTimer -= Time.deltaTime;
                                }
                            }
                        }
                    }

                    break;
                case CurrentState.Alert:
                    agent.SetDestination(player.position);
                    SetWalkingSpeed(1f);

                    if (FullySeesPlayer() || TouchingPlayer()) {
                        alertTimer = 15f;
                    } else {
                        if (alertTimer < 0f) {
                            vanishCounter = 5f;
                            currentState = CurrentState.Vanish;
                        } else {
                            alertTimer -= Time.deltaTime;
                        }
                    }

                    break;
                case CurrentState.Vanish:
                    if (TouchingPlayer() || FullySeesPlayer()) {
                        alertTimer = 15f;
                        currentState = CurrentState.Alert;
                    } else {
                        if (vanishCounter < 0f) {
                            currentState = CurrentState.Patrol;
                        } else {
                            vanishCounter -= Time.deltaTime;
                        }
                    }

                    break;
                default:
                    break;
            }
        }
    }

    void SetRigWeight(bool state) {
        rigBuilder.enabled = state;
    }

    bool TouchingPlayer() {
        return FullySeesPlayer() && InRange(transform.position, player.position, 5f);
    }

    public void SetWalkingSpeed(float speed) {
        anim.SetFloat("Speed", speed, 2f, 5f * Time.deltaTime);
    }

    private void OnAnimatorMove() {
        Vector3 targetVelocity = anim.deltaPosition / Time.deltaTime;

        if (agent.remainingDistance > agent.radius) {
            agent.speed = targetVelocity.magnitude;
        } else {
            agent.speed = 0f;
        }
    }

    void ShootPlayer() {
        if (PlayerController.isDead) return;
        if (reloadTime > 0f) return;

        reloadTime = 1f;

        shootSound.Play();

        if (Physics.Raycast(gunBarrel.position, gunBarrel.forward + Random.insideUnitSphere * inaccuracy, out RaycastHit hit)) {
            if (hit.collider.CompareTag("Player")) {
                PlayerController.instance.Die();
                AudioClip clip = fleshImpactSounds[Random.Range(0,fleshImpactSounds.Length)];
                AudioSource.PlayClipAtPoint(clip, hit.point);
            } else {
                AudioClip clip = groundImpactSounds[Random.Range(0,groundImpactSounds.Length)];
                AudioSource.PlayClipAtPoint(clip, hit.point);

                GameObject newImpact = LeanPool.Spawn(impactPrefab, hit.point, impactPrefab.transform.rotation);
                LeanPool.Despawn(newImpact, 10f);
            }
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

    bool FullySeesPlayer() {
        return SeesObject(player) && InRange(player.position, transform.position, sightRange) && InFov(player.position, fovRange);
    }
}
