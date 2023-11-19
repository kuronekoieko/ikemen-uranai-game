using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderManager : MonoBehaviour
{
    [SerializeField] Button infoButton;
    [SerializeField] Button purchaseButton;
    [SerializeField] Text purchaseButtonText;
    public void OnStart()
    {
        infoButton.onClick.AddListener(() =>
        {
            ChromeCustomTabs.OpenURL("https://www.google.co.jp");
        });

        purchaseButton.onClick.AddListener(() =>
        {
            UIManager.i.GetModal<PurchaseModal>().Open();
        });
    }

    private void Update()
    {
        purchaseButtonText.text = (SaveData.i.purchasedPoint + SaveData.i.freePoint).ToString("N0");
    }
}
