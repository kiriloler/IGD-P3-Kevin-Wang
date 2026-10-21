using System;
using UnityEngine;

public class HungerSystem : MonoBehaviour
{
    public bool IsHungry;
    public float Energy = 100f;

    private float timer = 0f;
    private float currentInterval;
    public float HungerThreshold = 60f;
    public FloatRange DecreaseInterval;
    public FloatRange DecreaseAmount;
    //hunting
    public FloatRange HuntTime;
    public FloatRange EatTime;
    public Transform currentPrey;
    public TriggerEventHook huntColliderHook;

    public Action<float> OnEnergyChange;
    public Action OnGetHungry;
    public Action OnGetNotHungry;
    public Action OnHuntingStart;
    public Action OnHuntingEnd;

    private OrganismSM owner;

    void Start()
    {
        owner = GetComponent<OrganismSM>();
        timer = Time.time;
        currentInterval = DecreaseInterval.Random();
        Debug.Log($"Decrease Interval of {currentInterval}");
    }


    void Update()
    {
        if (owner.IsAlive == false) return;
        if(Time.time - timer >= currentInterval)
        {
            //energy lost
            DecreaseEnergy();
            //start new energy lost interval
            timer = Time.time;
            currentInterval = DecreaseInterval.Random();
            Debug.Log($"Decrease Interval of {currentInterval}");
        }
    }

    void DecreaseEnergy()
    {
        bool wasNotHungry = Energy > 60;

        Energy = Mathf.Clamp(Energy - DecreaseAmount.Random(), 0, Energy);
        Debug.Log($"Decrease energy to {Energy}");
        OnEnergyChange?.Invoke(Energy);
        if (Energy == 0)
        {
            owner.Die();
            return;
        }
        if (wasNotHungry && Energy <= 60)
        {
            IsHungry = true;
            OnGetHungry?.Invoke();
        }
    }

    public void IncreaseEnergy(float amount)
    {
        Debug.Log($"Energy Increased by {amount}");
        bool wasHungry = Energy <= 60;

        Energy = Mathf.Clamp(Energy + amount, Energy, 100f);
        OnEnergyChange?.Invoke(Energy);
        if (wasHungry && Energy >= 60)
        {
            IsHungry = false;
            OnGetNotHungry?.Invoke();
        }
    }

    public void StartHunting()
    {
        OnHuntingStart?.Invoke();
    }

    public void EndHunting()
    {
        OnHuntingEnd?.Invoke();
    }
}
