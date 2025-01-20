using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.InquisitorState
{
    // public class LookAroundState : IState
    // {
    //     private float duration; // How long to stay in this state
    //     private float elapsedTime; // Time spent in the state
    //     private System.Action onTimeUp; // Callback for when time is up
    //     private StateMachine _stateMachine;
    //
    //     public LookAroundState(float duration, System.Action onTimeUp, StateMachine stateMachine)
    //     {
    //         _stateMachine = stateMachine;
    //         this.duration = duration;
    //         this.onTimeUp = onTimeUp;
    //     }
    //
    //     public void Enter()
    //     {
    //         Debug.Log("Entering Look Around State");
    //         elapsedTime = 0f; // Reset the timer
    //     }
    //
    //     public void Execute()
    //     {
    //         Debug.Log("Looking Around...");
    //
    //         // Increment the timer
    //         elapsedTime += Time.deltaTime;
    //
    //         // Check if time is up
    //         if (elapsedTime >= duration)
    //         {
    //             Debug.Log("Finished Looking Around");
    //             onTimeUp?.Invoke();
    //             _stateMachine.ChangeState(new PatrolState());
    //         }
    //     }
    //
    //     public void Exit()
    //     {
    //         Debug.Log("Exiting Look Around State");
    //     }
    // }

    public class PatrolState : IState
    {
        private Animator _animator;
        private NavMeshAgent _agent;
        private Transform[] _waypoints;
        private int _currentWaypointIndex;
        
        private bool _isLookingAround;
        private float lookingAroundDuration = 4f;
        private float elapsedTime;

        public PatrolState(Animator animator, NavMeshAgent navMeshAgent, Transform[] waypoints,
            int currentWaypointIndex)
        {
            _animator = animator;
            _agent = navMeshAgent;
            _waypoints = waypoints;
            _currentWaypointIndex = currentWaypointIndex;
        }

        public void Enter()
        {
            MoveToNextWaypoint();
            _animator.SetBool("isWalking", true);
        }

        public void Execute()
        {
            if (_isLookingAround)
            {
                Debug.Log("Looking Around...");
                LookAround();
                return;
            }
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                Debug.Log("Move to next waypoint (after looking around)...");
                _isLookingAround = true;
                return;
            }
            Debug.Log("Patrolling...");
        }

        private void LookAround()
        {
            _animator.SetBool("isLookingAround", true);
            elapsedTime += Time.deltaTime;

            // Check if time is up
            if (elapsedTime >= lookingAroundDuration)
            {
                _isLookingAround = false;
                elapsedTime = 0;
                MoveToNextWaypoint();
                _animator.SetBool("isLookingAround", false);
            }
        }

        public void Exit()
        {
            _animator.SetBool("isWalking", false);
        }

        void MoveToNextWaypoint()
        {
            if (_waypoints.Length == 0)
                return;
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
            _agent.SetDestination(_waypoints[_currentWaypointIndex].position);
        }
    }

    public class ChaseState : IState
    {
        private Transform target;
        private Animator animator;
        private NavMeshAgent agent;
        private System.Action onCatch;

        public ChaseState(Transform target, Animator animator, NavMeshAgent agent, System.Action onCatch)
        {
            this.target = target;
            this.animator = animator;
            this.agent = agent;
            this.onCatch = onCatch;
        }

        public void Enter()
        {
            animator.SetBool("isRunning", true);
            Debug.Log("Entering Chase State");
        }

        public void Execute()
        {
            if (target)
            {
                agent.SetDestination(target.position);
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    Debug.Log("KILL DEMON!");
                    onCatch?.Invoke();
                }

                Debug.Log("Chasing Target: " + target.name);
            }
            else
            {
                onCatch?.Invoke();
            }
        }

        public void Exit()
        {
            animator.SetBool("isRunning", false);
            Debug.Log("Exiting Chase State");
        }
    }
}