using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SaveDataObjects;
using Cysharp.Threading.Tasks;

public class PartnerElement : ObjectPoolingElement
{
    [SerializeField] Toggle toggle;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Button editButton;
    [SerializeField] Button deleteButton;
    public PartnerProfile PartnerProfile { get; private set; }
    public bool IsSelect => gameObject.activeSelf && toggle.isOn;

    public override void OnInstantiate()
    {
        toggle.group = GetComponentInParent<ToggleGroup>();

        editButton.AddListener(async () =>
        {
            await UniTask.DelayFrame(0);
            EditPartner();
        });

        deleteButton.AddListener(async () =>
        {
            bool success = await PopupManager.Instance.GetCommonPopup().ShowAsync(new DataBase.PopupText()
            {
                text = "この相手を削除しますか？",
                button_positive = "削除",
                button_negative = "戻る",
            });

            if (success)
            {
                gameObject.SetActive(false);
            }
        });
    }

    async void EditPartner()
    {
        PartnerProfile = await ScreenManager.Instance.Get<InputPartnerProfileScreen>().EditPartner(PartnerProfile);
        Show(PartnerProfile);
    }

    public void Show(PartnerProfile partnerProfile)
    {
        this.PartnerProfile = partnerProfile;
        nameText.text = partnerProfile.name;

    }
}
