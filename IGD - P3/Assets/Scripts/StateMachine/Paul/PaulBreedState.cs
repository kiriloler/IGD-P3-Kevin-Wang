using UnityEngine;
using UnityEngine.AI;

public class PaulBreedState : PaulStateBase
{
    float BreedTime;
    float TimeWhenEntered;

    public PaulBreedState(PaulSM player) : base(player)
    {
    }

    public override void Enter()
    {
        Paul.breedSystem.StartBreeding();

        BreedTime = Paul.breedSystem.BreedTime;
        Debug.Log($"Start Breeding! Time: {BreedTime}");
        TimeWhenEntered = Time.time;
    }

    public override void Update()
    {
        if (Time.time - TimeWhenEntered >= BreedTime)
        {
            BreedSystem partner = Paul.breedSystem.PartnerBreedSystem;
            //check if partner already finished breeding
            if (partner != null && Paul != null)
            {
                if (partner.PartnerBreedSystem == Paul.breedSystem)
                {
                    Vector2 breedPosition = (Paul.transform.position + partner.transform.position) / 2;
                    Services.organismSpawner.AddOrganism("Paul", breedPosition);
                }
            }
            Paul.breedSystem.StopBreeding();
            Paul.ChangeState(Paul.idleState, false);
        }
    }
    public override void Exit()
    {
        TimeWhenEntered = 0;
    }
}
