using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

public class DescriptionScreen : BaseScreen
{

    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;

    public override void OnStart(Camera uiCamera)
    {
        base.OnStart(uiCamera);
    }

    public void Open(string title, string description)
    {
        base.Open();

        titleText.text = title;
        descriptionText.text = description;
    }
    public override UniTask Close()
    {
        base.Close();
        return UniTask.DelayFrame(0);
    }
}
