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
        this.ObserveEveryValueChanged(async isShowBadgeAsync => await IsShowBadgeAsync())
            .Subscribe(async isShowBadgeAsync =>
            {
                bool isShowBadge = await isShowBadgeAsync;
                base.footerToggle.SetActiveBadge(isShowBadge);
            })
            .AddTo(gameObject);
    }


    async UniTask<bool> IsShowBadgeAsync()
    {
        if (PageManager.Instance == null) return false;
        bool isNotificationTodayHoroscope = await PageManager.Instance.Get<HomePage>().IsNotificationTodayHoroscope();
        bool isNotificationNextDayHoroscope = await PageManager.Instance.Get<HomePage>().IsNotificationNextDayHoroscope();
        return isNotificationTodayHoroscope || isNotificationNextDayHoroscope;
    }

    public void Home()
    {
        PageManager.Instance.Get<HomePage>().Open();
    }
}
