using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UranaishiButton : ObjectPoolingElement
{
    [SerializeField] Button button;
    [SerializeField] Image iconImage;
    [SerializeField] Text nameTxt;
    //[SerializeField] Text divinationsText;
    [SerializeField] Text messageText;
    [SerializeField] Text chargeText;
    [SerializeField] Text statusText;
    [SerializeField] ReviewElement reviewElement;

    public RectTransform rectTransform;
    Uranaishi uranaishi;

    public override void OnInstantiate()
    {
        button.onClick.AddListener(OnClickButton);
    }

    void OnClickButton()
    {
        UIManager.i.GetModal<UranaishiModal>().Open(uranaishi);
    }

    public void ShowData(Uranaishi uranaishi)
    {
        this.uranaishi = uranaishi;
        iconImage.sprite = null;
        uranaishi.GetIcon((sprite) =>
        {
            iconImage.sprite = sprite;
        });
        nameTxt.text = uranaishi.name;


        chargeText.text = uranaishi.GetChareText();
        //divinationsText.text = uranaishi.expertises.GetJoinedKeywords();
        messageText.SetLimitedText(uranaishi.message, 1);
        Review review = uranaishi.GetFirstReview();
        if (review != null)
        {
            reviewElement.ShowData(review, 2);
        }
        else
        {
            reviewElement.gameObject.SetActive(false);
        }


        statusText.text = uranaishi.GetStatusDisplayName();
    }

}
