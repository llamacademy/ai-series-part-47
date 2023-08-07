using UnityEngine;
using FSM;
using LlamAcademy.Sensors;
using UnityEngine.AI;

namespace LlamAcademy.FSM
{
    [RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Player Player;
        [SerializeField]
        private Spit SpitPrefab;
        [SerializeField]
        private ParticleSystem BounceImpactParticleSystem;
        
        [Header("Attack Config")]
        [SerializeField]
        [Range(0.1f, 5f)]
        private float AttackCooldown = 2;
        [SerializeField]
        [Range(1, 20f)]
        private float RollCooldown = 17;
        [SerializeField]
        [Range(1, 10f)]
        private float BounceCooldown = 10;
        
        [Header("Sensors")]
        [SerializeField]
        private PlayerSensor FollowPlayerSensor;
        [SerializeField]
        private PlayerSensor RangeAttackPlayerSensor;
        [SerializeField]
        private PlayerSensor MeleePlayerSensor;
        [SerializeField]
        private ImpactSensor RollImpactSensor;
        
        [Space]
        [Header("Debug Info")]
        [SerializeField]
        private bool IsInMeleeRange;
        [SerializeField]
        private bool IsInSpitRange;
        [SerializeField]
        private bool IsInChasingRange;
        [SerializeField]
        private float LastAttackTime;
        [SerializeField]
        private float LastBounceTime;
        [SerializeField]
        private float LastRollTime;
        
        private StateMachine<EnemyState, StateEvent> EnemyFSM;
        private Animator Animator;
        private NavMeshAgent Agent;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
            EnemyFSM = new();

            // Add States
            EnemyFSM.AddState(EnemyState.Idle, new IdleState(false, this));
            EnemyFSM.AddState(EnemyState.Chase, new ChaseState(true, this, Player.transform));
            EnemyFSM.AddState(EnemyState.Spit, new SpitState(true, this, SpitPrefab, OnAttack));
            EnemyFSM.AddState(EnemyState.Bounce, new BounceState(true, this, BounceImpactParticleSystem, OnBounce));
            EnemyFSM.AddState(EnemyState.Roll, new RollState(true, this, OnRoll));
            EnemyFSM.AddState(EnemyState.Attack, new AttackState(true, this, OnAttack));

            // Add Transitions
            EnemyFSM.AddTriggerTransition(StateEvent.DetectPlayer, new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase));
            EnemyFSM.AddTriggerTransition(StateEvent.LostPlayer, new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase,
                (transition) => IsInChasingRange
                                && Vector3.Distance(Player.transform.position, transform.position) > Agent.stoppingDistance)
            );
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle,
                (transition) => !IsInChasingRange
                                || Vector3.Distance(Player.transform.position, transform.position) <= Agent.stoppingDistance)
            );
            
            // Roll Transitions
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Roll, ShouldRoll, true));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Roll, ShouldRoll, true));
            EnemyFSM.AddTriggerTransition(StateEvent.RollImpact, 
                new Transition<EnemyState>(EnemyState.Roll, EnemyState.Bounce, null, true));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Roll, EnemyState.Chase, IsNotWithinIdleRange));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Roll, EnemyState.Idle, IsWithinIdleRange));
            
            // Bounce transitions
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Bounce, ShouldBounce));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Bounce, ShouldBounce));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Bounce, EnemyState.Chase, IsNotWithinIdleRange));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Bounce, EnemyState.Idle, IsWithinIdleRange));
            
            // Spit Transitions
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Spit, ShouldSpit, true));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Spit, ShouldSpit, true));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Spit, EnemyState.Chase, IsNotWithinIdleRange));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Spit, EnemyState.Idle, IsWithinIdleRange));
            
            // Attack Transitions
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack, ShouldMelee, true));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Attack, ShouldMelee, true));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Chase, IsNotWithinIdleRange));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Idle, IsWithinIdleRange));
            
            EnemyFSM.Init();
        }
        
        private void Start()
        {
            FollowPlayerSensor.OnPlayerEnter += FollowPlayerSensor_OnPlayerEnter;
            FollowPlayerSensor.OnPlayerExit += FollowPlayerSensor_OnPlayerExit;
            RangeAttackPlayerSensor.OnPlayerEnter += RangeAttackPlayerSensor_OnPlayerEnter;
            RangeAttackPlayerSensor.OnPlayerExit += RangeAttackPlayerSensor_OnPlayerExit;
            MeleePlayerSensor.OnPlayerEnter += MeleePlayerSensor_OnPlayerEnter;
            MeleePlayerSensor.OnPlayerExit += MeleePlayerSensor_OnPlayerExit;
            RollImpactSensor.OnCollision += RollImpactSensor_OnCollision;
        }
        
        private void RollImpactSensor_OnCollision(Collision Collision)
        {
            EnemyFSM.Trigger(StateEvent.RollImpact);
            LastRollTime = Time.time;
            LastAttackTime = Time.time;
            RollImpactSensor.gameObject.SetActive(false);
        }
        
        private void FollowPlayerSensor_OnPlayerExit(Vector3 LastKnownPosition)
        {
            EnemyFSM.Trigger(StateEvent.LostPlayer);
            IsInChasingRange = false;
        }

        private void FollowPlayerSensor_OnPlayerEnter(Transform Player)
        {
            EnemyFSM.Trigger(StateEvent.DetectPlayer);
            IsInChasingRange = true;
        }
        
        private bool ShouldRoll(Transition<EnemyState> Transition) => 
            LastRollTime + RollCooldown <= Time.time
                   && IsInChasingRange;

        private bool ShouldBounce(Transition<EnemyState> Transition) =>
            LastBounceTime + BounceCooldown <= Time.time
                   && IsInMeleeRange;

        private bool ShouldSpit(Transition<EnemyState> Transition) =>
            LastAttackTime + AttackCooldown <= Time.time
                   && !IsInMeleeRange
                   && IsInSpitRange;

        private bool ShouldMelee(Transition<EnemyState> Transition) =>
            LastAttackTime + AttackCooldown <= Time.time
                   && IsInMeleeRange;
        
        private bool IsWithinIdleRange(Transition<EnemyState> Transition) => 
            Agent.remainingDistance <= Agent.stoppingDistance;

        private bool IsNotWithinIdleRange(Transition<EnemyState> Transition) => 
            !IsWithinIdleRange(Transition);

        private void MeleePlayerSensor_OnPlayerExit(Vector3 LastKnownPosition) => IsInMeleeRange = false;

        private void MeleePlayerSensor_OnPlayerEnter(Transform Player) => IsInMeleeRange = true;

        private void RangeAttackPlayerSensor_OnPlayerExit(Vector3 LastKnownPosition) => IsInSpitRange = false;

        private void RangeAttackPlayerSensor_OnPlayerEnter(Transform Player) => IsInSpitRange = true;

        private void OnAttack(State<EnemyState, StateEvent> State)
        {
            transform.LookAt(Player.transform.position);
            LastAttackTime = Time.time;
        }

        private void OnBounce(State<EnemyState, StateEvent> State)
        {
            transform.LookAt(Player.transform.position);
            LastAttackTime = Time.time;
            LastBounceTime = Time.time;
        }

        private void OnRoll(State<EnemyState, StateEvent> State)
        {
            RollImpactSensor.gameObject.SetActive(true);
            transform.LookAt(Player.transform.position);
            LastRollTime = Time.time;
        }

        private void Update()
        {
            EnemyFSM.OnLogic();
        }
    }
}
