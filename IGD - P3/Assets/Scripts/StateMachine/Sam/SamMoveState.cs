using UnityEngine;
using UnityEngine.AI;

public class SamMoveState : SamStateBase
{
    float WanderTime = 0;
    float TimeWhenEntered = 0;
    public SamMoveState(SamSM player) : base(player)
    {
    }

    public override void Enter()
    {
        Sam.PlayAnimation("Move", 0f);
        WanderTime = Sam.WanderTime.Random();
        TimeWhenEntered = Time.time;
        Sam.hungerSystem.huntColliderHook.OnTriggerStay += OnPreyDetected;
        Sam.breedSystem.breedColliderHook.OnTriggerEnter += OnBreedPartnerDetected;
    }

    public override void Update()
    {
        if (Time.time - TimeWhenEntered > WanderTime)
        {
            if (Random.Range(0, 1) < 0.7f)
            {
                Sam.ChangeState(Sam.idleState, true);
            }
            else
            {
                Sam.ChangeState(Sam.moveState, true);
            }
            return;
        }
        //check if is moving
        if (Sam.agent.velocity.sqrMagnitude <= 0.01f)
        {
            Sam.agent.SetDestination(FindPointToMove());
        }
        Sam.UpdateFaceDirection();
    }
    public override void Exit()
    {
        Sam.StopAgent();
        WanderTime = 0;
        TimeWhenEntered = 0;
        Sam.hungerSystem.huntColliderHook.OnTriggerStay -= OnPreyDetected;
        Sam.breedSystem.breedColliderHook.OnTriggerEnter -= OnBreedPartnerDetected;
    }
}
