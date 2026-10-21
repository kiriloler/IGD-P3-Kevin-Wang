using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Slider PaulSlider;
    public Slider SamSlider;
    public Slider LuxieSlider;
    public Slider HarrySlider;
    public GameObject SettingPanel;
    public GameObject InGameUIs;

    private void Start()
    {
        Services.organismSpawner.OnEcosystemEnd += () => InGameUIs.SetActive(false);
    }

    public void StartExperiment()
    {
        Services.organismSpawner.SetCount("Paul", (int)PaulSlider.value);
        Services.organismSpawner.SetCount("Sam", (int)SamSlider.value);
        Services.organismSpawner.SetCount("Luxie", (int)LuxieSlider.value);
        Services.organismSpawner.SetCount("Harry", (int)HarrySlider.value);
        Services.organismSpawner.StartSpawning();
        SettingPanel.SetActive(false);
    }
}
