using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoveFortuneStar : MonoBehaviour
{
    [SerializeField] Image[] starImages;

    public void Show(float rate)
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            float d = rate - (float)i;

            starImages[i].fillAmount = Mathf.Clamp(d, 0, 1);
        }


    }

}
