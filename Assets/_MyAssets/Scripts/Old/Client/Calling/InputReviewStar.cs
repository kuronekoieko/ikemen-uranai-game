using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputReviewStar : MonoBehaviour
{
    Image image;
    InputReviewStarManager manager;
    int index;

    public void OnStart(InputReviewStarManager manager, int index)
    {
        this.manager = manager;
        this.index = index;
        image = GetComponent<Image>();
    }

    public void OnPointerEnter()
    {
        manager.ChangeView(index);
    }

    public void OnPointerExit()
    {

    }

    public void Highlight(bool enable)
    {
        if (enable)
        {
            image.color = Color.white;
        }
        else
        {
            image.color = Color.black;
        }
    }
}
