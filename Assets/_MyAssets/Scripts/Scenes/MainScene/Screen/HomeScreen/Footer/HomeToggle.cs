using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class HomeToggle : BaseFooterToggleController
{

    public override void OnStart()
    {
        base.OnStart();
        base.SetSelectedAction(Home);
        this.ObserveEveryValueChanged(isShowBadge => IsShowBadgeAsync())
            .Subscribe(isShowBadge =>
            {
                base.footerToggle.SetActiveBadge(isShowBadge);
            })
            .AddTo(gameObject);
    }


    bool IsShowBadgeAsync()
    {
        if (PageManager.Instance == null) return false;
        bool isNotificationTodayHoroscope = PageManager.Instance.Get<HomePage>().IsNotificationTodayHoroscope();
        bool isNotificationNextDayHoroscope = PageManager.Instance.Get<HomePage>().IsNotificationNextDayHoroscope();
        return isNotificationTodayHoroscope || isNotificationNextDayHoroscope;
    }

    public void Home()
    {
        PageManager.Instance.Get<HomePage>().Open();
    }
}
