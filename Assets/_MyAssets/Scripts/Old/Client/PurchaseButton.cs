using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PurchaseButton : MonoBehaviour
{
    Button button;
    public Action onClose { get; set; }

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            UIManager.i.GetModal<PurchaseModal>().Open();
            UIManager.i.GetModal<PurchaseModal>().onClose += onClose;
        });
    }


}
