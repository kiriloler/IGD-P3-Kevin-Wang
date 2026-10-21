using UnityEngine;

public class SamIdleState : SamStateBase
{
    float IdleTime = 0;
    float TimeWhenEntered = 0;
    public SamIdleState(SamSM player) : base(player)
    {
    }

    public override void Enter()
    {
        Sam.PlayAnimation("Idle", 0f);
        IdleTime = Sam.IdleTime.Random();
        TimeWhenEntered = Time.time;
        Sam.hungerSystem.huntColliderHook.OnTriggerStay += OnPreyDetected;
        Sam.breedSystem.breedColliderHook.OnTriggerEnter += OnBreedPartnerDetected;
    }

    public override void Update()
    {
        if(Time.time - TimeWhenEntered > IdleTime)
        {
            if(Random.Range(0f, 1f) < 0.7f)
            {
                Sam.ChangeState(Sam.moveState, true);
            }
            else
            {
                Sam.ChangeState(Sam.idleState, true);
            }
        }
    }
    public override void Exit()
    {
        IdleTime = 0;
        TimeWhenEntered = 0;
        Sam.hungerSystem.huntColliderHook.OnTriggerStay -= OnPreyDetected;
        Sam.breedSystem.breedColliderHook.OnTriggerEnter -= OnBreedPartnerDetected;
    }
}
