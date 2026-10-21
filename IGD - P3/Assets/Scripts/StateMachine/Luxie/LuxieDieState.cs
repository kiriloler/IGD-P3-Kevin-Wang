using UnityEngine;

public class LuxieDieState : LuxieStateBase
{
    float TimeWhenEntered = 0;
    public LuxieDieState(LuxieSM luxie) : base(luxie)
    {
    }

    public override void Enter()
    {
        Luxie.PlayAnimation("Die", 0f);
        TimeWhenEntered = Time.time;
    }

    public override void Update()
    {
        if(Time.time - TimeWhenEntered >= 4.5f)
        {
            if(Luxie.gameObject != null) Luxie.Vanish();
            return;
        }
    }
    public override void Exit()
    {
    }
}
