using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargingElement : ObjectPoolingElement
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] Button button;
    
    public override void OnInstantiate()
    {
    }
    
    public void OnStart()
    {
        button.onClick.AddListener(() =>
        {
        
        });
    }

    public void Show()
    {
    
    }
}
