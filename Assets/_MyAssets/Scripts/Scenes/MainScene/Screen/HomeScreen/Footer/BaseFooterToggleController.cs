using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class BaseFooterToggleController : MonoBehaviour
{
    [SerializeField] FooterToggleData footerToggleData;
    [SerializeField] ToggleGroup toggleGroup;
    protected FooterToggle footerToggle;
    public virtual void OnStart()
    {
        footerToggle = GetComponentInChildren<FooterToggle>();
        footerToggle.OnStart(footerToggleData, toggleGroup);
    }

    public virtual void SetSelectedAction(UnityAction onSelected)
    {
        footerToggle.SetSelectedAction(onSelected);
    }

    public void ToggleOn(bool isOn)
    {
        footerToggle.ToggleOn(isOn);
    }
}

[Serializable]
public class FooterToggleData
{
    public string name;
    public Sprite onSprite;
    public Sprite offSprite;
    public Sprite nameSprite;
    public bool interactable = true;
    public bool active = true;
}
