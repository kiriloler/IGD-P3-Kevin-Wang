using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class LuxieStateBase : StateBase
{
    protected LuxieSM Luxie;
    public LuxieStateBase(LuxieSM Luxie)
    {
        this.Luxie = Luxie;
    }

    protected Vector2 FindPointToMove()
    {
        int i = 0;
        while (i < 9)
        {
            Vector2 RandomPt = Luxie.transform.position + Random.insideUnitSphere * Luxie.WanderRadius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(RandomPt, out hit, 5f, Luxie.agent.areaMask))
            {
                return hit.position;
            }
            i++;
        }
        return Vector2.zero;
    }
}