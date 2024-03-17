using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FooterToggle : MonoBehaviour
{

    [SerializeField] Toggle toggle;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image badgeImage;
    public void OnStart(FooterToggleData footerToggleData)
    {
        text.text = footerToggleData.name;
        toggle.image.sprite = footerToggleData.offSprite;
        toggle.graphic.GetComponent<Image>().sprite = footerToggleData.onSprite;
    }


}
