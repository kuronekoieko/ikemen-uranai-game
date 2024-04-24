using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Events;

public class FooterToggle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField] Toggle toggle;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image badgeImage;
    [SerializeField] Image nameImage;

    readonly float duration = 0.2f;


    public void OnStart(FooterToggleData footerToggleData, ToggleGroup toggleGroup)
    {
        toggle.group = toggleGroup;
        // text.text = footerToggleData.name;
        nameImage.sprite = footerToggleData.nameSprite;
        toggle.image.sprite = footerToggleData.offSprite;
        if (toggle.graphic.TryGetComponent(out Image graphicImage))
        {
            graphicImage.sprite = footerToggleData.onSprite;
        }

        toggle.interactable = footerToggleData.interactable;
        toggle.gameObject.SetActive(footerToggleData.active);

        SetActiveBadge(false);
        // これがないと、最後のだけtrueになってしまう
        // 1フレーム待っても変更されない
        toggle.isOn = false;
    }

    public virtual void SetSelectedAction(UnityAction onSelected)
    {
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                onSelected.Invoke();
                AudioManager.Instance.PlayOneShot(AudioID.BtnClick_Positive);
            }
        });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (toggle.interactable == false) return;
        transform.DOScale(Vector3.one * 1.1f, duration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, duration);
    }
    public void ToggleOn(bool isOn)
    {
        toggle.isOn = isOn;
    }

    public void SetActiveBadge(bool active)
    {
        badgeImage.gameObject.SetActive(active);
    }
}


