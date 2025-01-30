using UnityEngine;
using UnityEngine.AI;

namespace Inquisitor.State
{
    public class InqPatrolState : AbstractInquisitorState
    {
        private Transform[] _waypoints;
        private int _currentWaypointIndex = 0;
        private NavMeshAgent _navMeshAgentRef;
        private Animator _animatorRef;

        public override void Update(InquisitorController inquisitor)
        {
            if (inquisitor.enemyToChase)
            {
                inquisitor.ChangeState(inquisitor.ChaseState);
            }
            else
            {
                if (_navMeshAgentRef.remainingDistance <= _navMeshAgentRef.stoppingDistance)
                {
                    inquisitor.ChangeState(inquisitor.LookAroundState);
                }
            }
        }

        public override void EnterState(InquisitorController inquisitor)
        {
            Debug.Log("Entering Patrol State");
            _waypoints = inquisitor.waypoints.ToArray();
            if (!_navMeshAgentRef)
            {
                _navMeshAgentRef = inquisitor.GetComponent<NavMeshAgent>();
            }

            if (!_animatorRef)
            {
                _animatorRef = inquisitor.GetComponent<Animator>();
            }

            _animatorRef.SetBool("isWalking", true);

            MoveToNextWaypoint();
        }

        public override void ExitState(InquisitorController inquisitor)
        {
            _animatorRef.SetBool("isWalking", false);
        }

        void MoveToNextWaypoint()
        {
            if (_waypoints.Length == 0)
                return;
            _navMeshAgentRef.SetDestination(_waypoints[_currentWaypointIndex].position);
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
        }
    }
}