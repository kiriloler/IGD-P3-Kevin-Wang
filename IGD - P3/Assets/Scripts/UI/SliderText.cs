using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderText : MonoBehaviour
{
    Slider slider;
    public TextMeshProUGUI textMeshPro;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider = GetComponent<Slider>();
        textMeshPro.text = slider.value.ToString();
        slider.onValueChanged.AddListener((v) =>  { textMeshPro.text = ((int)v).ToString(); });
    }
}
