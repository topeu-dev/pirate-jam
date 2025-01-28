using UnityEngine;
using UnityEngine.AI;

namespace Inquisitor.State
{
    public class InqRunToHelpState : AbstractInquisitorState
    {
        private Animator _animatorRef;
        private NavMeshAgent _navMeshAgentRef;

        public override void Update(InquisitorController inquisitor)
        {
            _navMeshAgentRef.SetDestination(inquisitor.runToHelpPoint.position);
            if (Vector3.Distance(_navMeshAgentRef.transform.position, inquisitor.runToHelpPoint.position)
                <= 3f)
            {
                inquisitor.ChangeState(inquisitor.PatrolState);
            }
        }

        public override void EnterState(InquisitorController inquisitor)
        {
            if (!_animatorRef)
            {
                _animatorRef = inquisitor.GetComponent<Animator>();
            }

            if (!_navMeshAgentRef)
            {
                _navMeshAgentRef = inquisitor.GetComponent<NavMeshAgent>();
            }

            _navMeshAgentRef.SetDestination(inquisitor.runToHelpPoint.position);
            _animatorRef.SetBool("isRunning", true);
        }

        public override void ExitState(InquisitorController inquisitor)
        {
            _animatorRef.SetBool("isRunning", false);
            _animatorRef.SetBool("isWalking", false);
        }
    }
}