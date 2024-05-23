using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using DataBase;
using Cysharp.Threading.Tasks;

public class LoveFortunePerson : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI nameText;
    // [SerializeField] TextMeshProUGUI compatibilityMessageText;


    public void Show(Sprite iconSprite, string name)
    {
        iconImage.sprite = iconSprite;
        nameText.text = name;
    }

}
