using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;


public class FooterToggle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public Toggle toggle;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image badgeImage;
    readonly float duration = 0.2f;


    public void OnStart(FooterToggleData footerToggleData)
    {
        text.text = footerToggleData.name;
        toggle.image.sprite = footerToggleData.offSprite;
        if (toggle.graphic.TryGetComponent(out Image graphicImage))
        {
            graphicImage.sprite = footerToggleData.onSprite;
        }
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) footerToggleData.onSelected.Invoke();
        });
        toggle.interactable = footerToggleData.interactable;
        toggle.gameObject.SetActive(footerToggleData.active);

        badgeImage.gameObject.SetActive(false);
        // これがないと、最後のだけtrueになってしまう
        // 1フレーム待っても変更されない
        toggle.isOn = false;
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

}
