using FSM;
using System;
using UnityEngine;

namespace LlamAcademy.FSM
{
    public class BounceState : EnemyStateBase
    {
        private ParticleSystem BounceParticleSystem;

        public BounceState(
            bool needsExitTime,
            Enemy Enemy,
            ParticleSystem BounceParticleSystem,
            Action<State<EnemyState, StateEvent>> onEnter,
            float ExitTime = 0.33f) : base(needsExitTime, Enemy, ExitTime, onEnter)
        {
            this.BounceParticleSystem = BounceParticleSystem;
        }

        public override void OnEnter()
        {
            Agent.isStopped = true;
            base.OnEnter();
            Animator.Play("Bounce");
            BounceParticleSystem.Play();
        }
    }
}
