using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UranaishiModal : BaseModal
{


    [SerializeField] Transform contentTf;
    [SerializeField] CallUranaishiButton callUranaishiButton;
    [SerializeField] Button scheduleButton;
    BaseUranaishiProfile[] baseUranaishiProfiles;
    Uranaishi uranaishi;

    public override void OnStart()
    {
        base.OnStart();
        StartLayoutElements();
        callUranaishiButton.OnStart();
        scheduleButton.onClick.AddListener(OnClickScheduleButton);
    }

    void StartLayoutElements()
    {
        baseUranaishiProfiles = contentTf.GetComponentsInChildren<BaseUranaishiProfile>(true);
        foreach (var item in baseUranaishiProfiles)
        {
            item.gameObject.SetActive(true);
            item.OnStart();
        }
    }


    public void Open(Uranaishi uranaishi)
    {
        this.uranaishi = uranaishi;
        base.OpenAnim();
        foreach (var item in baseUranaishiProfiles)
        {
            item.OnOpen(uranaishi);
        }
        callUranaishiButton.OnOpen(uranaishi);
    }



    void OnClickScheduleButton()
    {
        UIManager.i.GetModal<ScheduleModal>().Open(uranaishi);
    }
}
