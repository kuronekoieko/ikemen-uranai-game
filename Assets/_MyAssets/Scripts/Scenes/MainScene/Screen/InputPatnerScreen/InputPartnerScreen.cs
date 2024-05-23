using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SaveDataObjects;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InputPartnerScreen : BaseScreen
{
    [SerializeField] PartnerPool partnerPool;
    [SerializeField] Button addPartnerButton;
    [SerializeField] Button nextButton;

    public override void OnStart(Camera uiCamera)
    {
        base.OnStart(uiCamera);
        partnerPool.OnStart();
        nextButton.AddListener(async () =>
        {
            var partnerElement = partnerPool.list.FirstOrDefault(partnerElement => partnerElement.IsSelect);

            if (partnerElement == null)
            {
                await PopupManager.Instance.GetCommonPopup().ShowAsync(new DataBase.PopupText()
                {
                    text = "相手が選択されていません",
                    button_positive = "OK",
                });
            }
            else
            {
                ScreenManager.Instance.Get<LoveFortuneScreen>().Open(partnerElement.PartnerProfile);
            }
        });
        addPartnerButton.AddListener(async () =>
        {
            await UniTask.DelayFrame(0);
            CreatePartner();
        });
    }

    async void CreatePartner()
    {
        var partnerProfile = await ScreenManager.Instance.Get<InputPartnerProfileScreen>().CreatePartner();
        partnerPool.Add(partnerProfile);
    }

    public override void Open()
    {
        base.Open();
    }

    public override UniTask Close()
    {
        return base.Close();
    }

}
