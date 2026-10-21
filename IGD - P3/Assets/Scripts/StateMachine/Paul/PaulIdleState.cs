using UnityEngine;

public class PaulIdleState : PaulStateBase
{
    float IdleTime = 0;
    float TimeWhenEntered = 0;
    public PaulIdleState(PaulSM player) : base(player)
    {
    }

    public override void Enter()
    {
        Paul.PlayAnimation("Idle", 0f);
        IdleTime = Paul.IdleTime.Random();
        TimeWhenEntered = Time.time;
        Debug.Log($"Paul Starts to Idle! Idle Time: {IdleTime}");
        Paul.hungerSystem.huntColliderHook.OnTriggerStay += OnPreyDetected;
        Paul.breedSystem.breedColliderHook.OnTriggerEnter += OnBreedPartnerDetected;
    }

    public override void Update()
    {
        if(Time.time - TimeWhenEntered > IdleTime)
        {
            if(Random.Range(0, 1) < 0.7f)
            {
                Paul.ChangeState(Paul.moveState, true);
            }
            else
            {
                Paul.ChangeState(Paul.idleState, true);
            }
        }
    }
    public override void Exit()
    {
        IdleTime = 0;
        TimeWhenEntered = 0;
        Paul.hungerSystem.huntColliderHook.OnTriggerStay -= OnPreyDetected;
        Paul.breedSystem.breedColliderHook.OnTriggerEnter -= OnBreedPartnerDetected;
    }
}
