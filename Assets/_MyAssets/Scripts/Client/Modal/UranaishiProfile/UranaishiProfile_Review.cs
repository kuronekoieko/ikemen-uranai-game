using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UranaishiProfile_Review : BaseUranaishiProfile
{
    [SerializeField] ReviewManager reviewManager;
    [SerializeField] Button moreButton;
    readonly int showReviewCount = 6;
    Uranaishi uranaishi;
    public override void OnStart()
    {
        reviewManager.OnStart();
        moreButton.onClick.AddListener(() =>
        {
            UIManager.i.GetModal<ReviewListModal>().Open(uranaishi);
        });

    }

    public override void OnOpen(Uranaishi uranaishi)
    {
        this.uranaishi = uranaishi;
        var reviews = uranaishi.GetOrderedReviews();
        var showingReviews = reviews.Take(showReviewCount).ToArray();

        reviewManager.ShowElements(showingReviews);

        bool isShowBoreButton = reviews.Count() > showReviewCount;

        moreButton.gameObject.SetActive(isShowBoreButton);
        if (isShowBoreButton == false) return;
        moreButton.transform.SetAsLastSibling();

    }

}
