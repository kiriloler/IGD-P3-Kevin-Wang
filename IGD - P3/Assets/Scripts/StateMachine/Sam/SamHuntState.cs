using UnityEngine;
using UnityEngine.AI;

public class SamHuntState : SamStateBase
{
    private enum HuntState
    {
        Chasing,
        Eating
    }

    float HuntTime = 0;
    float EatTime = 0;
    float TimeWhenEntered = 0;
    OrganismSM preySM;

    HuntState state;
    HuntState State
    {
        get
        {
            return state;
        }
        set
        {
            switch (value)
            {
                case HuntState.Chasing:
                    Sam.PlayAnimation("Move", 0f);
                    if(HuntTime == 0)
                    {
                        HuntTime = Sam.hungerSystem.HuntTime.Random();
                        TimeWhenEntered = Time.time;
                    }
                    break;
                case HuntState.Eating:
                    Sam.PlayAnimation("Idle", 0f);
                    EatTime = Sam.hungerSystem.EatTime.Random();
                    Debug.Log($"EatTime: {EatTime}");
                    TimeWhenEntered = Time.time;
                    break;
            }
            state = value;
        }
    }

    public SamHuntState(SamSM player) : base(player)
    {
    }

    public override void Enter()
    {
        Sam.hungerSystem.StartHunting();
        Debug.Log($"Start Hunting! Prey: {Sam.hungerSystem.currentPrey}");
        State = HuntState.Chasing;
        
        preySM = Sam.hungerSystem.currentPrey.GetComponent<OrganismSM>();
    }

    public override void Update()
    {
        if(Sam.hungerSystem.currentPrey == null || preySM.IsAlive == false)
        {
            Sam.ChangeState(Sam.idleState, false);
            return;
        }
        switch (State)
        {
            case HuntState.Chasing:
                //time check
                if(Time.time - TimeWhenEntered >= HuntTime)
                {
                    Sam.ChangeState(Sam.idleState);
                    return;
                }

                Sam.agent.SetDestination(Sam.hungerSystem.currentPrey.position);
                Sam.UpdateFaceDirection();
                if (Vector2.Distance(Sam.transform.position, Sam.hungerSystem.currentPrey.position) <= 1)
                {
                    Sam.StopAgent();
                    State = HuntState.Eating;
                    break;
                }
                break;
            case HuntState.Eating:
                //time check
                if (Time.time - TimeWhenEntered >= EatTime)
                {
                    float energy = preySM.hungerSystem.Energy;
                    Sam.hungerSystem.IncreaseEnergy(energy);
                    preySM.Die();
                    Sam.ChangeState(Sam.idleState);
                    return;
                }
                break;
        }
    }
    public override void Exit()
    {
        Sam.hungerSystem.EndHunting();
        Sam.StopAgent();
        Sam.hungerSystem.currentPrey = null;
        HuntTime = 0;
        EatTime = 0;
        TimeWhenEntered = 0;
        preySM = null;
    }
}
