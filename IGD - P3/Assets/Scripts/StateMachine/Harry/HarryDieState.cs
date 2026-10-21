using UnityEngine;

public class HarryDieState : HarryStateBase
{
    float TimeWhenEntered = 0;
    public HarryDieState(HarrySM Harry) : base(Harry)
    {
    }

    public override void Enter()
    {
        Harry.PlayAnimation("Wither", 0f);
        TimeWhenEntered = Time.time;
    }

    public override void Update()
    {
        if(Time.time - TimeWhenEntered >= 4.5f)
        {
            Harry.Vanish();
            return;
        }
    }
    public override void Exit()
    {
    }
}
