using UnityEngine;
using UnityEngine.AI;

public class LuxieSM : OrganismSM
{
    #region properties
    public FloatRange VelocityY;
    //colliders
    public TriggerEventHook attackColliderHook;
    public Collider2D attackCollider { get; private set; }
    #endregion

    #region components
    #endregion

    #region states
    public LuxieIdleState idleState { get; private set; }
    public LuxieMoveState moveState { get; private set; }
    public LuxieDieState dieState { get; private set; }
    #endregion

    protected override void Start()
    {
        base.Start();

        attackCollider = attackColliderHook.GetComponent<Collider2D>();
        attackCollider.enabled = false;

        //state init
        idleState = new LuxieIdleState(this);
        moveState = new LuxieMoveState(this);
        dieState = new LuxieDieState(this);
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

    public void StartJump()
    {
        if(CurrentState == moveState)
        {
            moveState.StartJumping();
        }
    }

    public override void Die()
    {
        base.Die();
        ChangeState(dieState, false);
    }
}