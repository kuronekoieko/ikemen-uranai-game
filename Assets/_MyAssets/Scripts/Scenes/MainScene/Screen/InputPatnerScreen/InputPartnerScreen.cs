using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

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
            ScreenManager.Instance.Get<LoveFortuneScreen>().Open();
            await UniTask.DelayFrame(0);
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
