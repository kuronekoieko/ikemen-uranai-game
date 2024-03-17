using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

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

    public void Home()
    {
        PageManager.Instance.Get<HomePage>().Open();
    }
    public void Story()
    {
        PageManager.Instance.Get<StoryPage>().Open();
    }
    public void Horoscope()
    {
        PageManager.Instance.Get<HoroscopePage>().Open();
    }
    public void Gacha()
    {
        PageManager.Instance.Get<GachaPage>().Open();
    }
    public void Character()
    {
        PageManager.Instance.Get<CharactersPage>().Open();
    }

}

[Serializable]
public class FooterToggleData
{
    public string name;
    public Sprite onSprite;
    public Sprite offSprite;
    public UnityEvent onSelected;
    public bool interactable = true;
    public bool active = true;
}