using System.Collections.Generic;
using Inquisitor.State;
using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace Inquisitor
{
    public class InquisitorController : MonoBehaviour
    {
        public float timeToLookAround = 1.5f;
        public float timeToKill = 2f;

        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private Transform _currentTarget;

        private AbstractInquisitorState _currentState;
        internal InqPatrolState PatrolState = new InqPatrolState();
        internal InqLookAroundState LookAroundState = new InqLookAroundState();
        internal InqKillingDemonState KillingDemonState = new InqKillingDemonState();
        internal InqChaseState ChaseState = new InqChaseState();
        internal InqRunToHelpState RunToHelpState = new InqRunToHelpState();

        internal GameObject enemyToChase;

        // internal DemonAoeTest enemyToChase;
        public List<Transform> waypoints;

        public Transform runToHelpPoint;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.updatePosition = false;

            _animator = GetComponent<Animator>();
            _animator.applyRootMotion = true;
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
            if (runToHelpPoint)
            {
                _currentState = RunToHelpState;
                _currentState.EnterState(this);
            }
            else
            {
                _currentState = PatrolState;
                _currentState.EnterState(this);
            }

            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            _currentState.Update(this);
        }

        public void ChangeState(AbstractInquisitorState newState)
        {
            _currentState?.ExitState(this);
            _currentState = newState;
            _currentState.EnterState(this);
        }

        public void KillDemon()
        {
            var demonController = enemyToChase.GetComponent<DemonAoeTest>();
            demonController.Death();
            enemyToChase = null;
            EventManager.DemonChargeEvent.OnDemonKilledByInqEvent?.Invoke(this);
        }

        public void setTargetToChase(GameObject targetToChase)
        {
            enemyToChase = targetToChase;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            foreach (Transform waypoint in waypoints)
            {
                Gizmos.DrawSphere(waypoint.position, 0.5f);
            }
        }
    }
}