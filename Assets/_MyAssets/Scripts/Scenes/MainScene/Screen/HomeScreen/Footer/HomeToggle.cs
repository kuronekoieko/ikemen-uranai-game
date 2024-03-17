using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HomeToggle : BaseFooterToggleController
{

    public override void OnStart()
    {
        base.OnStart();
        base.SetSelectedAction(Home);
        this.ObserveEveryValueChanged(isShowBadge => IsShowBadge())
            .Subscribe(isShowBadge => base.footerToggle.SetActiveBadge(isShowBadge))
            .AddTo(gameObject);
    }

    bool IsShowBadge()
    {
        if (PageManager.Instance == null) return false;
        bool isNotificationTodayHoroscope = PageManager.Instance.Get<HomePage>().IsNotificationTodayHoroscope();
        bool isNotificationNextDayHoroscope = PageManager.Instance.Get<HomePage>().IsNotificationNextDayHoroscope();
        return isNotificationTodayHoroscope && isNotificationNextDayHoroscope;
    }

    public void Home()
    {
        PageManager.Instance.Get<HomePage>().Open();
    }
}
