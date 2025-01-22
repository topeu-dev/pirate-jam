using UnityEngine;

namespace Inquisitor.State
{
    public class InqKillingDemonState : AbstractInquisitorState
    {
        private float elapsedTime;
        private Animator _animatorRef;
        
        public override void Update(InquisitorController inquisitor)
        {
            if (!inquisitor.enemyToChase)
            {
                inquisitor.ChangeState(inquisitor.PatrolState);
            }
            
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= inquisitor.timeToKill)
            {
                inquisitor.KillDemon();
                Debug.Log("KILL called");
                inquisitor.ChangeState(inquisitor.PatrolState);
            }
        }

        public override void EnterState(InquisitorController inquisitor)
        {
            Debug.Log("Inquisitor entered Killing state");
            _animatorRef = inquisitor.GetComponent<Animator>();
            elapsedTime = 0f;
            _animatorRef.SetBool("Kill", true);
        }

        public override void ExitState(InquisitorController inquisitor)
        {
            _animatorRef.SetBool("Kill", false);
        }
    }
}