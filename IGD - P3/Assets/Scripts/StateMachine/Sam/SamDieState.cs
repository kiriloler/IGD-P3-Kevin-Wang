using UnityEngine;

public class SamDieState : SamStateBase
{
    float TimeWhenEntered = 0;
    public SamDieState(SamSM sam) : base(sam)
    {
    }

    public override void Enter()
    {
        Sam.GetComponent<GrowthSystem>().stopGrowing = true;
        Sam.PlayAnimation("Die", 0f);
        TimeWhenEntered = Time.time;
    }

    public override void Update()
    {
            if(Time.time - TimeWhenEntered >= 4.5f)
            {
                if(Sam.gameObject != null) Sam.Vanish();
                return;
            }
    }
    public override void Exit()
    {
    }
}
