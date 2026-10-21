using UnityEngine;
using UnityEngine.AI;

public class PaulSM : OrganismSM
{
    #region properties
    //colliders
    public TriggerEventHook attackColliderHook;
    public Collider2D attackCollider { get; private set; }
    #endregion

    #region components
    public BreedSystem breedSystem { get; private set; }
    #endregion

    #region states
    public PaulIdleState idleState { get; private set; }
    public PaulMoveState moveState { get; private set; }
    public PaulHuntState huntState { get; private set; }
    public PaulBreedState breedState { get; private set; }
    public PaulDieState dieState { get; private set; }
    #endregion

    protected override void Start()
    {
        base.Start();

        attackCollider = attackColliderHook.GetComponent<Collider2D>();
        attackCollider.enabled = false;

        breedSystem = GetComponent<BreedSystem>();

        //state init
        idleState = new PaulIdleState(this);
        moveState = new PaulMoveState(this);
        huntState = new PaulHuntState(this);
        breedState = new PaulBreedState(this);
        dieState = new PaulDieState(this);
        ChangeState(idleState, false);
    }

    public void StartAttack()
    {
        attackCollider.enabled = true;
    }

    public void EndAttack()
    {
        attackCollider.enabled = false;
    }

    public override void Die()
    {
        base.Die();
        ChangeState(dieState, false);
    }
}