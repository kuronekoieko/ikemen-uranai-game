using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleContentTexts : MonoBehaviour
{
    [SerializeField] Text titleTxt;
    [SerializeField] Text contentTxt;

    public void SetTexts(string title, string content)
    {
        titleTxt.text = title;
        contentTxt.text = content;
        gameObject.SetActive(true);
    }

    public void SetTexts(string title, string content, int lineLimit)
    {
        titleTxt.text = title;
        contentTxt.SetLimitedText(content, lineLimit);
        gameObject.SetActive(true);
    }
}
