using UnityEngine;
using UnityEngine.AI;
public class SamSM : OrganismSM
{
    #region properties
    #endregion

    #region components
    public BreedSystem breedSystem {  get; private set; }
    #endregion

    #region states
    public SamIdleState idleState { get; private set; }
    public SamMoveState moveState { get; private set; }
    public SamDieState dieState { get; private set; }
    public SamHuntState huntState { get; private set; }
    public SamBreedState breedState { get; private set; }
    #endregion

    protected override void Start()
    {
        base.Start();

        breedSystem = GetComponent<BreedSystem>();

        //state init
        idleState = new SamIdleState(this);
        moveState = new SamMoveState(this);
        dieState = new SamDieState(this);
        huntState = new SamHuntState(this);
        breedState = new SamBreedState(this);
        
        ChangeState(idleState, false);
    }

    public override void Die()
    {
        base.Die();
        ChangeState(dieState, false);
    }
}