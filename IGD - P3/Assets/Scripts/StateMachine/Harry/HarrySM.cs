using UnityEngine;

public class HarrySM : OrganismSM
{
    public HarryIdleState idleState {  get; private set; }
    public HarryDieState dieState {  get; private set; }

    protected override void Start()
    {
        base.Start();

        idleState = new HarryIdleState(this);
        dieState = new HarryDieState(this);

        ChangeState(idleState, false);
    }

    public override void Die()
    {
        base.Die();
        ChangeState(dieState, false);
    }
}
