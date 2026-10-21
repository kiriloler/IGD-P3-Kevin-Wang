using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class OrganismUI : MonoBehaviour
{
    public GameObject organism;

    private HungerSystem hungerSystem;
    float maxEnergy;
    private BreedSystem breedSystem;

    public Image HealthBar_Content;
    public Image HealthBar_Base;
    public Color healthyColor;
    public Color hungryColor;
    public Image HeartIcon;
    public Image HuntIcon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HealthBar_Content.color = healthyColor;
        HealthBar_Base.color = healthyColor;
        hungerSystem = organism.GetComponent<HungerSystem>();
        breedSystem = organism.GetComponent<BreedSystem>();
        
        if(hungerSystem != null)
        {
            maxEnergy = hungerSystem.Energy;
            hungerSystem.OnEnergyChange += OnEnergyChange;
            hungerSystem.OnGetHungry += OnGetHungry;
            hungerSystem.OnGetNotHungry += OnGetNotHungry;
            hungerSystem.OnHuntingStart += OnHuntingStart;
            hungerSystem.OnHuntingEnd += OnHuntingEnd;
        }
        if(breedSystem != null)
        {
            breedSystem.OnBreedStart += OnBreedStart;
            breedSystem.OnBreedEnd += OnBreedEnd;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnergyChange(float energy)
    {
        HealthBar_Content.DOFillAmount(energy / maxEnergy, 0.5f);
    }

    void OnGetHungry()
    {
        Debug.Log("HUNRGY");
        HealthBar_Base.color = hungryColor;
        HealthBar_Content.color = hungryColor;
    }

    void OnGetNotHungry()
    {
        Debug.Log("Not Hungry");
        HealthBar_Base.color = healthyColor;
        HealthBar_Content.color = healthyColor;
    }

    void OnHuntingStart()
    {
        HuntIcon.gameObject.SetActive(true);
    }

    void OnHuntingEnd()
    {
        HuntIcon.gameObject.SetActive(false);
    }
    
    void OnBreedStart()
    {
        HeartIcon.gameObject.SetActive(true);
    }

    void OnBreedEnd()
    {
        HeartIcon.gameObject.SetActive(false);
    }
}
