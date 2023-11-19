using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReviewStarManager : MonoBehaviour
{

    InputReviewStar[] inputReviewStars;
    public int starCount { get; private set; }
    public void OnStart()
    {
        inputReviewStars = GetComponentsInChildren<InputReviewStar>(true);

        for (int i = 0; i < inputReviewStars.Length; i++)
        {
            inputReviewStars[i].OnStart(this, i);
        }
    }

    public void ChangeView(int selectedIndex)
    {
        starCount = selectedIndex + 1;
        for (int i = 0; i < inputReviewStars.Length; i++)
        {
            inputReviewStars[i].Highlight(i <= selectedIndex);
        }
    }
}
