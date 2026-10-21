using UnityEngine;
using UnityEngine.AI;

public class PaulMoveState : PaulStateBase
{
    float WanderTime = 0;
    float TimeWhenEntered = 0;
    public PaulMoveState(PaulSM player) : base(player)
    {
    }

    public override void Enter()
    {
        Paul.PlayAnimation("Idle", 0f);
        WanderTime = Paul.WanderTime.Random();
        TimeWhenEntered = Time.time;
        Debug.Log($"Paul Starts to Move! Move Time: {WanderTime}");
        Paul.hungerSystem.huntColliderHook.OnTriggerStay += OnPreyDetected;
        Paul.breedSystem.breedColliderHook.OnTriggerEnter += OnBreedPartnerDetected;
    }

    public override void Update()
    {
        if(Time.time - TimeWhenEntered > WanderTime)
        {
            if(Random.Range(0, 1) < 0.7f)
            {
                Paul.ChangeState(Paul.idleState, true);
            }
            else
            {
                Paul.ChangeState(Paul.moveState, true);
            }
            return;
        }
        //check if is moving
        if(Paul.agent.velocity.sqrMagnitude <= 0.01f)
        {
            Paul.agent.SetDestination(FindPointToMove());
        }
        Paul.UpdateFaceDirection();
    }
    public override void Exit()
    {
        Paul.StopAgent();
        WanderTime = 0;
        TimeWhenEntered = 0;
        Paul.hungerSystem.huntColliderHook.OnTriggerStay -= OnPreyDetected;
        Paul.breedSystem.breedColliderHook.OnTriggerEnter -= OnBreedPartnerDetected;
    }
}
