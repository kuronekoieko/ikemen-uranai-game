using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UranauPage : BasePageManager
{

    [SerializeField] UranaishiButtonManager uranaishiButtonManager;
    [SerializeField] Button uranaishiListButton;

    public override void OnStart()
    {
        base.SetPageAction(0);
        uranaishiButtonManager.OnStart();
        uranaishiListButton.onClick.AddListener(() => UIManager.i.GetModal<UranaishiListModal>().Open());

        uranaishiButtonManager.ShowButtons(UIManager.i.uranaishiAry.Take(10).ToArray());
    }

    public override void OnUpdate()
    {

    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
    }
}
