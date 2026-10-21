using System;
using UnityEngine;

public class BreedSystem : MonoBehaviour
{
    public float BreedTime = 5;
    public float BreedCoolDown = 10;
    private float TimeWhenCoolDown = 0;
    private bool onBreedCoolDown = false;
    public bool IsReadyToBreed = false;
    public BreedSystem PartnerBreedSystem;
    public TriggerEventHook breedColliderHook;

    public Action OnBreedStart;
    public Action OnBreedEnd;

    public void StartBreeding()
    {
        OnBreedStart?.Invoke();
    }

    private void Start()
    {
        TimeWhenCoolDown = Time.time;
    }

    private void Update()
    {
        if(onBreedCoolDown && Time.time - TimeWhenCoolDown >= BreedCoolDown)
        {
            onBreedCoolDown = false;
            IsReadyToBreed = true;
        }
    }

    public void StopBreeding()
    {
        IsReadyToBreed = false;
        PartnerBreedSystem = null;
        onBreedCoolDown = true;
        TimeWhenCoolDown = Time.time;

        OnBreedEnd?.Invoke();
    }
}
