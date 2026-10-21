using UnityEngine;
using UnityEngine.AI;

public class PaulHuntState : PaulStateBase
{
    private enum HuntState
    {
        Chasing,
        Attacking,
        Eating
    }

    float HuntTime = 0;
    float EatTime = 0;
    float TimeWhenEntered = 0;
    bool killedPrey = false;
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
                    Paul.PlayAnimation("Idle", 0f);
                    if(HuntTime == 0)
                    {
                        HuntTime = Paul.hungerSystem.HuntTime.Random();
                        TimeWhenEntered = Time.time;
                    }
                    break;
                case HuntState.Attacking:
                    Debug.Log("Start Attack Animation");
                    Paul.PlayAnimation("Attack", 0f);
                    Paul.attackColliderHook.OnTriggerEnter += OnAttackHit;
                    break;
                case HuntState.Eating:
                    Paul.PlayAnimation("Idle", 0f);
                    EatTime = Paul.hungerSystem.EatTime.Random();
                    Debug.Log($"EatTime: {EatTime}");
                    TimeWhenEntered = Time.time;
                    break;
            }
            state = value;
        }
    }

    public PaulHuntState(PaulSM player) : base(player)
    {
    }

    public override void Enter()
    {
        Paul.hungerSystem.StartHunting();
        Debug.Log($"Start Hunting! Prey: {Paul.hungerSystem.currentPrey}");
        State = HuntState.Chasing;
        
        preySM = Paul.hungerSystem.currentPrey.GetComponent<OrganismSM>();
    }

    public override void Update()
    {
        if(killedPrey == false && (Paul.hungerSystem.currentPrey == null || (preySM != null && preySM.IsAlive == false)))
        {
            Paul.ChangeState(Paul.idleState, false);
            return;
        }
        switch (State)
        {
            case HuntState.Chasing:
                //time check
                if(Time.time - TimeWhenEntered >= HuntTime)
                {
                    Paul.ChangeState(Paul.idleState);
                    return;
                }

                Paul.agent.SetDestination(Paul.hungerSystem.currentPrey.position);
                Paul.UpdateFaceDirection();
                if (Vector2.Distance(Paul.transform.position, Paul.hungerSystem.currentPrey.position) <= 1)
                {
                    Paul.StopAgent();
                    State = HuntState.Attacking;
                    break;
                }
                break;
            case HuntState.Attacking:
                AnimatorStateInfo currentInfo = Paul.animator.GetCurrentAnimatorStateInfo(0);
                if(currentInfo.normalizedTime >= 0.9f)
                {
                    if (killedPrey)
                    {
                        State = HuntState.Eating;
                    }
                    else
                    {
                        State = HuntState.Chasing;
                    }
                }
                break;
            case HuntState.Eating:
                //time check
                if (Time.time - TimeWhenEntered >= EatTime)
                {
                    if(preySM != null)
                    {
                        float energy = preySM.hungerSystem.Energy;
                        Paul.hungerSystem.IncreaseEnergy(Random.Range(energy / 2, energy));
                        preySM.Vanish();
                    }
                    Paul.ChangeState(Paul.idleState);
                    return;
                }
                break;
        }
    }
    public override void Exit()
    {
        Paul.hungerSystem.EndHunting();
        Paul.StopAgent();
        Paul.attackColliderHook.OnTriggerEnter -= OnAttackHit;
        Paul.hungerSystem.currentPrey = null;
        HuntTime = 0;
        EatTime = 0;
        TimeWhenEntered = 0;
        killedPrey = false;
        preySM = null;
    }

    void OnAttackHit(Collider2D collider)
    {
        if (preySM != null && killedPrey == false && preySM.IsAlive == true)
        {
            preySM.Die();
            Debug.Log("Successfully killed.");
            killedPrey = true;
        }
    }
}
