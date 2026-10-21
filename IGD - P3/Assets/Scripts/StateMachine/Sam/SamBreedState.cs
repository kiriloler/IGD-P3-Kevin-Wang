using UnityEngine;
using UnityEngine.AI;

public class SamBreedState : SamStateBase
{
    float BreedTime;
    float TimeWhenEntered;

    public SamBreedState(SamSM player) : base(player)
    {
    }

    public override void Enter()
    {
        Sam.breedSystem.StartBreeding();

        Sam.PlayAnimation("Idle", 0f);
        BreedTime = Sam.breedSystem.BreedTime;
        Debug.Log($"Start Breeding! Time: {BreedTime}");
        TimeWhenEntered = Time.time;
    }

    public override void Update()
    {
        if(Time.time - TimeWhenEntered >= BreedTime)
        {
            BreedSystem partner = Sam.breedSystem.PartnerBreedSystem;
            //check if partner already finished breeding
            if(partner != null && partner.PartnerBreedSystem == Sam.breedSystem)
            {
                Vector2 breedPosition = (Sam.transform.position + partner.transform.position) / 2;
                Services.organismSpawner.AddOrganism("Sam", breedPosition);
            }
            Sam.ChangeState(Sam.idleState, false);
            Sam.breedSystem.StopBreeding();
        }
    }
    public override void Exit()
    {
        TimeWhenEntered = 0;
    }
}
