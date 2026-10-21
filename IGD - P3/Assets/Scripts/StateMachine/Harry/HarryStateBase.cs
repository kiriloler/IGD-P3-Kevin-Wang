using UnityEngine;
using UnityEngine.AI;

public class HarryStateBase : StateBase
{
    protected HarrySM Harry;
    public HarryStateBase(HarrySM Harry)
    {
        this.Harry = Harry;
    }
}
