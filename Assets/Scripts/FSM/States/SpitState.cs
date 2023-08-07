using FSM;
using System;
using UnityEngine;
using UnityEngine.Pool;

namespace LlamAcademy.FSM
{
    public class SpitState : EnemyStateBase
    {
        private Spit Prefab;
        private ObjectPool<Spit> Pool;

        public SpitState(
            bool needsExitTime, 
            Enemy Enemy, 
            Spit Prefab, 
            Action<State<EnemyState, StateEvent>> onEnter,
            float ExitTime = 0.33f) : base(needsExitTime, Enemy, ExitTime, onEnter)
        {
            this.Prefab = Prefab;
            Pool = new(CreateObject, GetObject, ReleaseObject);
        }

        private Spit CreateObject()
        {
            return GameObject.Instantiate(Prefab);
        }

        private void GetObject(Spit Instance)
        {
            Instance.transform.forward = Enemy.transform.forward;
            Instance.transform.position = Enemy.transform.position + Enemy.transform.forward + Vector3.up * 1.5f;
            Instance.gameObject.SetActive(true);
        }

        private void ReleaseObject(Spit Instance)
        {
            Instance.gameObject.SetActive(false);
        }

        public override void OnEnter()
        {
            Agent.isStopped = true;
            base.OnEnter();
            Animator.Play("Attack");
            Pool.Get();
        }
    }
}
