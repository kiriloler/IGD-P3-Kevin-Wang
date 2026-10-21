using UnityEngine;

public class HarryIdleState : HarryStateBase
{
    public HarryIdleState(HarrySM Harry) : base(Harry)
    {
    }

    public override void Enter()
    {
        Harry.PlayAnimation("Idle", 0f);
    }
}
