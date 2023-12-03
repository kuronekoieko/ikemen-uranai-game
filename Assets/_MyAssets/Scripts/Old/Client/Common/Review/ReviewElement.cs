using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReviewElement : ObjectPoolingElement
{

    [SerializeField] Text titleTxt;
    [SerializeField] Text contentTxt;
    [SerializeField] Image pickUpImage;
    [SerializeField] ReviewStarHighlight reviewStarHighlight;

    public void ShowData(Review review)
    {
        contentTxt.text = review.text;
        titleTxt.text = review.GetTitleText();
        reviewStarHighlight.HighlightStars(review.starCount);
        if (pickUpImage) pickUpImage.gameObject.SetActive(review.isPickUp);
    }

    public void ShowData(Review review, int lineLimit)
    {
        contentTxt.SetLimitedText(review.text, lineLimit);
        titleTxt.text = review.GetTitleText();
        reviewStarHighlight.HighlightStars(review.starCount);
        if (pickUpImage) pickUpImage.gameObject.SetActive(review.isPickUp);
    }

    public override void OnInstantiate()
    {

    }

}
