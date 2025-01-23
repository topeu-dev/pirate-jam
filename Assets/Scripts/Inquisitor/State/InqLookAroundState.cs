using UnityEngine;
using UnityEngine.AI;

namespace Inquisitor.State
{
    public class InqLookAroundState : AbstractInquisitorState
    {
        private Animator _animatorRef;
        private NavMeshAgent _navMeshAgentRef;
        private float _elapsedTime;

        public override void Update(InquisitorController inquisitor)
        {
            if (inquisitor.enemyToChase)
            {
                inquisitor.ChangeState(inquisitor.ChaseState);
            }

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= inquisitor.timeToLookAround)
            {
                inquisitor.ChangeState(inquisitor.PatrolState);
            }
        }

        public override void EnterState(InquisitorController inquisitor)
        {
            Debug.Log("Inquisitor entered LookAroundState");
            if (!_navMeshAgentRef)
            {
                _navMeshAgentRef = inquisitor.GetComponent<NavMeshAgent>();
            }

            if (!_animatorRef)
            {
                _animatorRef = inquisitor.GetComponent<Animator>();
            }

            _navMeshAgentRef.isStopped = true;
            _animatorRef.SetBool("isLookingAround", true);
            _elapsedTime = 0f;
        }

        public override void ExitState(InquisitorController inquisitor)
        {
            _animatorRef.SetBool("isLookingAround", false);
            _navMeshAgentRef.isStopped = false;
        }
    }
}