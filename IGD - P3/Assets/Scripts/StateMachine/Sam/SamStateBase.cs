using UnityEngine;
using UnityEngine.AI;

public class SamStateBase : StateBase
{
    protected SamSM Sam;
    public SamStateBase(SamSM sam)
    {
        this.Sam = sam;
    }

    protected void OnPreyDetected(Collider2D collider)
    {
        if (Sam.hungerSystem.IsHungry && Sam.hungerSystem.currentPrey == null && collider.GetComponent<OrganismSM>().IsAlive == true)
        {
            NavMeshPath path = new NavMeshPath();
            Sam.agent.CalculatePath(collider.transform.position, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                Sam.hungerSystem.currentPrey = collider.transform;
                Sam.ChangeState(Sam.huntState);
            }
        }
    }

    protected void OnBreedPartnerDetected(Collider2D collider)
    {
        if (Sam.breedSystem != null && Sam.breedSystem.IsReadyToBreed)
        {
            //if the collided organism also dont have a partner then we part up
            BreedSystem partner = collider.GetComponent<BreedSystem>();
            SamSM partnerSM = partner.GetComponent<SamSM>();
            if (partner != null && partnerSM.IsAlive && partner.IsReadyToBreed && partner.PartnerBreedSystem == null)
            {
                Sam.breedSystem.PartnerBreedSystem = partner;
                partner.PartnerBreedSystem = Sam.breedSystem;
                Sam.ChangeState(Sam.breedState, false);
                partnerSM.ChangeState(partnerSM.breedState, false);
                return;
            }
        }
    }

    protected Vector2 FindPointToMove()
    {
        int i = 0;
        while (i < 9)
        {
            Vector2 RandomPt = Sam.transform.position + Random.insideUnitSphere * Sam.WanderRadius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(RandomPt, out hit, 5f, Sam.agent.areaMask))
            {
                return hit.position;
            }
            i++;
        }
        return Vector2.zero;
    }
}
