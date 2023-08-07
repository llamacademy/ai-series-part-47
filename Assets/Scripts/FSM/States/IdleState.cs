using UnityEngine;

namespace LlamAcademy.FSM
{
    public class IdleState : EnemyStateBase
    {
        private float AnimationLoopCount = 0;

        public IdleState(bool needsExitTime, Enemy Enemy) : base(needsExitTime, Enemy) { }

        public override void OnEnter()
        {
            base.OnEnter();
            Agent.isStopped = true;
            Animator.Play("Idle_A");
        }

        public override void OnLogic()
        {
            AnimatorStateInfo state = Animator.GetCurrentAnimatorStateInfo(0);

            if (state.normalizedTime >= AnimationLoopCount + 1)
            {
                float value = Random.value;
                if (value < 0.95f)
                {
                    if (!state.IsName("Idle_A"))
                    {
                        AnimationLoopCount = 0;
                    }
                    else
                    {
                        AnimationLoopCount++;
                    }
                    Animator.Play("Idle_A");
                }
                else if (value < 0.975f)
                {
                    if (!state.IsName("Idle_B"))
                    {
                        AnimationLoopCount = 0;
                    }
                    else
                    {
                        AnimationLoopCount++;
                    }
                    Animator.Play("Idle_B");
                }
                else
                {
                    if (!state.IsName("Idle_C"))
                    {
                        AnimationLoopCount = 0;
                    }
                    else
                    {
                        AnimationLoopCount++;
                    }
                    Animator.Play("Idle_C");
                }
            }
            
            base.OnLogic();
        }
    }
}
