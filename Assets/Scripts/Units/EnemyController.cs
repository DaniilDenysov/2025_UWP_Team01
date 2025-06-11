using System.Collections;
using System.Collections.Generic;
using TowerDeffence.AI.Data;
using TowerDeffence.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using TowerDeffence.HealthSystem;
using TowerDeffence.Interfaces;

public class EnemyController : MonoBehaviour, IDamagable
{
    public EnemyStateMachine StateMachine { get; private set; }
    public StateMachineContext Context { get; private set; }
    public AttackSO AttackData => attackController.attackSO;
    public GameObject currentTarget { get; private set; }

    private EnemyMovement movementComponent;
    private AttackController attackController;
    private IEnemyState dieState;
    private Animator animator;
    private HealthPresenter healthPresenter;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        healthPresenter = GetComponent<HealthPresenter>();
        movementComponent = GetComponent<EnemyMovement>();
        attackController = GetComponent<AttackController>();
        movementComponent.Initialize();

        dieState = new EnemyDieState();
        StateMachine = new EnemyStateMachine();
        Context = new StateMachineContext(StateMachine, animator, this, movementComponent, attackController, dieState);
        StateMachine.SetContext(Context);
    }

    private void Start()
    {
        StateMachine.Initialize(movementComponent);
    }

    private void Update()
    {
        if (IsDead()) return;

        currentTarget = attackController.GetClosestTarget();

        StateMachine.CurrentState?.Execute(Context);
    }

    public void RequestStateChange(IEnemyState requestedState)
    {StateMachine.ChangeState(requestedState);
    }

    public void Warp(Vector3 position)
    {
        movementComponent.Warp(position);
    }

    public bool DoDamage(uint damageAmount)
    {
        if (IsDead()) return true;
        bool isNowDead = healthPresenter.DoDamage(damageAmount);
        if (isNowDead)
        {
            StateMachine.ChangeState(dieState);
        }
        return isNowDead;
    }

    public uint GetCurrentHealthPoints()
    {
        return healthPresenter.GetCurrentHealthPoints();
    }

    public bool IsDead()
    {
        if (healthPresenter == null) return true;
        return healthPresenter.GetCurrentHealthPoints() <= 0;
    }

    public IEnumerator DestroyAfterAnimation(Animator animator, string animationStateName)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}