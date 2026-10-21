using UnityEngine;

public class LuxieIdleState : LuxieStateBase
{
    float IdleTime = 0;
    float TimeWhenEntered = 0;
    public LuxieIdleState(LuxieSM luxie) : base(luxie)
    {
    }

    public override void Enter()
    {
        Luxie.PlayAnimation("Idle", 0f);
        IdleTime = Luxie.IdleTime.Random();
        TimeWhenEntered = Time.time;
        Debug.Log($"Luxie Starts to Idle! Idle Time: {IdleTime}");
    }

    public override void Update()
    {
        if(Time.time - TimeWhenEntered > IdleTime)
        {
            if(Random.Range(0, 1) < 0.7f)
            {
                Luxie.ChangeState(Luxie.moveState, true);
            }
            else
            {
                Luxie.ChangeState(Luxie.idleState, true);
            }
        }
    }
    public override void Exit()
    {
        IdleTime = 0;
        TimeWhenEntered = 0;
    }
}
