using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviewStarHighlight : MonoBehaviour
{
    [SerializeField] Image[] starImages;
    [SerializeField] Text starCountTxt;
    public void HighlightStars(float starCount)
    {
        if (starCountTxt) starCountTxt.text = starCount.ToString("F1");
        int floor = Mathf.FloorToInt(starCount);
        float remainder = starCount - floor;
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].type = Image.Type.Filled;
            starImages[i].fillMethod = Image.FillMethod.Horizontal;
            starImages[i].fillAmount = starCount - i;
        }
    }
}
