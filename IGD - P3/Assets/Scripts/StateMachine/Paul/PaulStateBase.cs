using UnityEngine;
using UnityEngine.AI;

public class PaulStateBase : StateBase
{
    protected PaulSM Paul;
    public PaulStateBase(PaulSM paul)
    {
        this.Paul = paul;
    }

    protected void OnPreyDetected(Collider2D collider)
    {
        if (Paul.hungerSystem.IsHungry && Paul.hungerSystem.currentPrey == null && collider.GetComponent<OrganismSM>().IsAlive == true)
        {
            NavMeshPath path = new NavMeshPath();
            Paul.agent.CalculatePath(collider.transform.position, path);
            if(path.status == NavMeshPathStatus.PathComplete)
            {
                Paul.hungerSystem.currentPrey = collider.transform;
                Paul.ChangeState(Paul.huntState);
            }
        }
    }

    protected void OnBreedPartnerDetected(Collider2D collider)
    {
        if (Paul.breedSystem != null && Paul.breedSystem.IsReadyToBreed)
        {
            //if the collided organism also dont have a partner then we part up
            BreedSystem partner = collider.GetComponent<BreedSystem>();
            PaulSM partnerSM = collider.GetComponent<PaulSM>();
            Debug.Log($"is partner's partner null: {partner.PartnerBreedSystem == null}");
            if (partner != null && partnerSM.IsAlive && partner.IsReadyToBreed && partner.PartnerBreedSystem == null)
            {
                Paul.breedSystem.PartnerBreedSystem = partner;
                partner.PartnerBreedSystem = Paul.breedSystem;
                Paul.ChangeState(Paul.breedState, false);
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
            Vector2 RandomPt = Paul.transform.position + Random.insideUnitSphere * Paul.WanderRadius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(RandomPt, out hit, 5f, Paul.agent.areaMask))
            {
                return hit.position;
            }
            i++;
        }
        return Vector2.zero;
    }
}
