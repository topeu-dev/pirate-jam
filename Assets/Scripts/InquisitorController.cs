using System;
using UnityEngine;
using UnityEngine.AI;
using StateMachine.InquisitorState;

public class InquisitorController : MonoBehaviour
{
    public Transform[] waypoints;
    public float detectionRadius = 10f;
    public float timeToLookAround = 1.5f;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    // private int currentWaypointIndex = 0;

    private StateMachine.StateMachine stateMachine;
    private FieldOfView _fieldOfView;

    private PatrolState patrolState;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updatePosition = false;

        animator = GetComponent<Animator>();
        animator.applyRootMotion = true;

        _fieldOfView = GetComponentInChildren<FieldOfView>();

        patrolState = new PatrolState(animator, navMeshAgent, waypoints, 0);
    }

    private void OnAnimatorMove()
    {
        Vector3 animatorRootPos = animator.rootPosition;
        animatorRootPos.y = navMeshAgent.nextPosition.y;
        navMeshAgent.nextPosition = animatorRootPos;
        transform.position = animatorRootPos;
    }

    void Start()
    {
        stateMachine = new StateMachine.StateMachine();
        stateMachine.ChangeState(patrolState);

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        stateMachine.Update();

        Transform enemyTarget = FindTargetInFov();
        if (enemyTarget)
        {
            stateMachine.ChangeState(new ChaseState(enemyTarget, animator, navMeshAgent,
                () =>
                {
                    if (enemyTarget)
                    {
                        Destroy(enemyTarget.gameObject);
                    }

                    stateMachine.ChangeState(patrolState);
                }));
        }

        // else
        // {
        //     stateMachine.ChangeState(new PatrolState(animator, stateMachine));
        // }
        //
        // // if no enemy and closeTo stopping distance
        // if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        // {
        //     stateMachine.ChangeState(new LookAroundState(timeToLookAround, OnTimeUp));
        //     return;
        // }
    }

    // private void OnTimeUp()
    // {
    //     Debug.Log("OnTimeUp");
    //     MoveToNextWaypoint();
    //     stateMachine.ChangeState(new PatrolState(animator));
    // }


    private void OnDrawGizmosSelected()
    {
        // Draw detection radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.green;
        foreach (Transform waypoint in waypoints)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f); // Visualize waypoints
        }
    }

    private Transform FindTargetInFov()
    {
        if (_fieldOfView.visibleTargets.Count <= 0)
            return null;

        foreach (var t in _fieldOfView.visibleTargets)
        {
            if (!t || !t.gameObject.CompareTag("Demon"))
                continue;

            return t.transform;
        }

        return null;
    }
}