using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ReviewListModal : BaseModal
{
    [SerializeField] ReviewManager reviewManager;
    [SerializeField] ReviewStarHighlight reviewStarHighlight;

    readonly int showReviewCount = 20;

    public override void OnStart()
    {
        base.OnStart();
        reviewManager.OnStart();
    }

    public void Open(Uranaishi uranaishi)
    {
        base.OpenAnim(ModalType.Horizontal);

        var reviews = uranaishi.GetOrderedReviews();

        reviewManager.ShowElements(reviews);

        reviewStarHighlight.HighlightStars(uranaishi.GetReviewAvr());
    }
}
