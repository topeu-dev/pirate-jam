using UnityEngine;
using UnityEngine.AI;

namespace Inquisitor.State
{
    public class InqChaseState : AbstractInquisitorState
    {
        private Animator _animatorRef;
        private NavMeshAgent _navMeshAgentRef;


        public override void Update(InquisitorController inquisitor)
        {
            if (inquisitor.enemyToChase)
            {
                _navMeshAgentRef.SetDestination(inquisitor.enemyToChase.transform.position);
                if (Vector3.Distance(_navMeshAgentRef.transform.position, inquisitor.enemyToChase.transform.position)
                    <= 5f)
                {
                    inquisitor.ChangeState(inquisitor.KillingDemonState);
                }
            }
            else
            {
                inquisitor.ChangeState(inquisitor.LookAroundState);
            }
        }

        public override void EnterState(InquisitorController inquisitor)
        {
            Debug.Log("Inquisitor entered Killing state");
            if (!_animatorRef)
            {
                _animatorRef = inquisitor.GetComponent<Animator>();
            }

            if (!_navMeshAgentRef)
            {
                _navMeshAgentRef = inquisitor.GetComponent<NavMeshAgent>();
            }

            _navMeshAgentRef.SetDestination(inquisitor.enemyToChase.transform.position);
            _animatorRef.SetBool("isRunning", true);
        }

        public override void ExitState(InquisitorController inquisitor)
        {
            _animatorRef.SetBool("isRunning", false);
            _animatorRef.SetBool("isWalking", false);
        }
    }
}