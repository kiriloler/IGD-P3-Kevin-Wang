using UnityEngine;
using TMPro;

public class ResultUI : MonoBehaviour
{
    public TextMeshProUGUI ResultText;
    public TextMeshProUGUI PaulText;
    public TextMeshProUGUI SamText;
    public TextMeshProUGUI LuxieText;
    public TextMeshProUGUI HarryText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Services.organismSpawner.OnEcosystemEnd += ShowResult;
        gameObject.SetActive(false);
    }

    private void ShowResult()
    {
        gameObject.SetActive(true);
        ResultText.text = $"The ecosystem lasted for {Services.organismSpawner.TimeLasted.ToString("0.00")} seconds.";
        PaulText.text = $"Max. Count: {Services.organismSpawner.Settings.Find(setting => setting.name == "Paul").maxCount}";
        SamText.text = $"Max. Count: {Services.organismSpawner.Settings.Find(setting => setting.name == "Sam").maxCount}";
        LuxieText.text = $"Max. Count: {Services.organismSpawner.Settings.Find(setting => setting.name == "Luxie").maxCount}";
        HarryText.text = $"Max. Count: {Services.organismSpawner.Settings.Find(setting => setting.name == "Harry").maxCount}";
    }
}
