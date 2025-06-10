using System.Collections;
using System.Collections.Generic;
using TowerDeffence.AI.Data;
using TowerDeffence.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using TowerDeffence.HealthSystem;

public class EnemyController : MonoBehaviour
{
    public EnemyStateMachine StateMachine { get; private set; }
    public StateMachineContext Context { get; private set; }

    private EnemyMovement movementComponent;
    private AttackController attackController;
    private IEnemyState idleState;
    private IEnemyState dieState;

    private Animator animator;
    private HealthPresenter healthPresenter;

    public AttackController AttackController { get => attackController; }

    public Transform Target { get; set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        healthPresenter = GetComponent<HealthPresenter>();
        movementComponent = GetComponent<EnemyMovement>();
        attackController = GetComponent<AttackController>();

        dieState = new EnemyDieState();

        StateMachine = new EnemyStateMachine();
        Context = new StateMachineContext(StateMachine, animator, this, movementComponent, attackController, dieState);
    }

    private void Start()
    {
        StateMachine.Initialize(idleState);
    }

    private void Update()
    {
        StateMachine.CurrentState?.Execute(Context);
    }

    public void DoDamage(uint damageAmount)
    {
        if (healthPresenter.GetCurrentHealthPoints() <= 0) return;

        bool isNowDead = healthPresenter.DoDamage(damageAmount);
        if (isNowDead)
        {
            StateMachine.ChangeState(dieState);
        }
    }

    public bool IsDead(uint damageAmount)
    {
        return healthPresenter.GetCurrentHealthPoints() <= damageAmount;
    }

    public IEnumerator DestroyAfterAnimation(Animator animator, string animationStateName)
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
