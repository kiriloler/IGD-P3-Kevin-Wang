using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Diagnostics.CodeAnalysis;

[Serializable]
public class Selection
{
    public string name;
    public GameObject placeHolder;
    public GameObject prefab;
    public float cd;
    private bool isOnCd = false;
    private float timeWhenEnteredCd;
    public Image coolDownImg;

    public void StartCd()
    {
        isOnCd = true;
        timeWhenEnteredCd = Time.time;
    }

    public void UpdateCd()
    {
        if (isOnCd)
        {
            coolDownImg.fillAmount = Mathf.Clamp(1 - (Time.time - timeWhenEnteredCd) / cd, 0, 1);
            if(coolDownImg.fillAmount <= 0)
            {
                isOnCd = false;
            }
        }
    }

    public bool CanSelect()
    {
        return isOnCd == false;
    }
}
public class ExternalSystem : MonoBehaviour
{
    public AudioSource audio;
    private Selection currentSelection;
    public List<Selection> selections;

    public GameObject selectionHint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnSelect(string name)
    {
        if (currentSelection != null)
        {
            currentSelection.placeHolder.SetActive(false);
            currentSelection = null;
        }
        Selection selection = selections.Find(s => s.name == name);
        if (selection != null && selection.CanSelect())
        {
            currentSelection = selection;
            selectionHint.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Selection selection in selections)
        {
            selection.UpdateCd();
        }

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (currentSelection != null)
        {
            if (currentSelection.placeHolder.activeSelf == false) currentSelection.placeHolder.SetActive(true);

            Vector2 mousePositionOnScreen = Input.mousePosition;
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
            currentSelection.placeHolder.transform.position = mouseWorldPos;

            if (Input.GetMouseButtonUp(0))
            {
                audio.Play();
                currentSelection.placeHolder.SetActive(false);
                currentSelection.StartCd();
                if(currentSelection.name != "Hand")
                {
                    Services.organismSpawner.AddOrganism(currentSelection.name, mouseWorldPos);
                }
                else
                {
                    Instantiate(currentSelection.prefab, mouseWorldPos, Quaternion.identity);
                }
                currentSelection = null;
                selectionHint.SetActive(false);
            }

            if (Input.GetMouseButtonUp(1))
            {
                currentSelection.placeHolder.SetActive(false);
                selectionHint.SetActive(false);
                currentSelection = null;
            }
        }
    }
}
