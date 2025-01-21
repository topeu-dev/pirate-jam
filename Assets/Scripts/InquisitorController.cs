using StateMachine.InquisitorState;
using UnityEngine;
using UnityEngine.AI;

public class InquisitorController : MonoBehaviour
{
    public float timeToLookAround = 1.5f;
    public Transform[] waypoints;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private StateMachine.StateMachine _stateMachine;
    private FieldOfView _fieldOfView;

    private PatrolState _patrolState;
    private Transform _currentTarget;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updatePosition = false;

        _animator = GetComponent<Animator>();
        _animator.applyRootMotion = true;

        _fieldOfView = GetComponentInChildren<FieldOfView>();

        _patrolState = new PatrolState(_animator, _navMeshAgent, waypoints, 0);
    }

    private void OnAnimatorMove()
    {
        Vector3 animatorRootPos = _animator.rootPosition;
        animatorRootPos.y = _navMeshAgent.nextPosition.y;
        _navMeshAgent.nextPosition = animatorRootPos;
        transform.position = animatorRootPos;
    }

    void Start()
    {
        _stateMachine = new StateMachine.StateMachine();
        _stateMachine.ChangeState(_patrolState);

        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        _stateMachine.Update();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (Transform waypoint in waypoints)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
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

    public void ChaseDemon(GameObject enemyTarget)
    {
        _stateMachine.ChangeState(new ChaseState(enemyTarget.transform, _animator, _navMeshAgent,
            () =>
            {
                if (enemyTarget)
                {
                    Destroy(enemyTarget.gameObject);
                }

                _stateMachine.ChangeState(_patrolState);
            }));
    }
}