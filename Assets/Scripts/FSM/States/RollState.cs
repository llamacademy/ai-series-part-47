using FSM;
using System;
using UnityEngine;

namespace LlamAcademy.FSM
{
    public class RollState : EnemyStateBase
    {
        public RollState(
            bool needsExitTime,
            Enemy Enemy,
            Action<State<EnemyState, StateEvent>> onEnter,
            float ExitTime = 3f) : base(needsExitTime, Enemy, ExitTime, onEnter) { }

        public override void OnEnter()
        {
            Agent.isStopped = true;
            base.OnEnter();
            Animator.Play("Roll");
        }

        public override void OnLogic()
        {
            Agent.Move(1.5f * Agent.speed * Time.deltaTime * Agent.transform.forward);
            base.OnLogic();
        }
    }
}
