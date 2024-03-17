using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class HomeFooter : MonoBehaviour
{
    FooterToggle[] footerToggles;
    [SerializeField] FooterToggleData[] footerToggleDatas;

    public void OnStart()
    {
        footerToggles = GetComponentsInChildren<FooterToggle>(true);
        foreach (var footerToggle in footerToggles)
        {
            footerToggle.gameObject.SetActive(false);
            // footerToggle.OnStart();
        }
        FooterToggle footerTogglePrefab = footerToggles[0];

        foreach (var footerToggleData in footerToggleDatas)
        {
            FooterToggle footerToggle = Instantiate(footerTogglePrefab, transform);
            footerToggle.OnStart(footerToggleData);
            footerToggle.gameObject.SetActive(true);
        }
    }
}

[Serializable]
public class FooterToggleData
{
    public string name;
    public Sprite onSprite;
    public Sprite offSprite;
}