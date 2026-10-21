using UnityEngine;

public class PaulDieState : PaulStateBase
{
    float TimeWhenEntered = 0;
    public PaulDieState(PaulSM player) : base(player)
    {
    }

    public override void Enter()
    {
        Paul.PlayAnimation("Die", 0f);
        TimeWhenEntered = Time.time;
    }

    public override void Update()
    {
        if(Time.time - TimeWhenEntered >= 4.5f)
        {
            if(Paul.gameObject != null) Paul.Vanish();
            return;
        }
    }
    public override void Exit()
    {
    }
}
