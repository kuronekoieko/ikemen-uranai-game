using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KuchikomiElement : ObjectPoolingElement
{
    [SerializeField] Button uranaishiButton;
    [SerializeField] Image iconImage;
    [SerializeField] Text nameTxt;
    [SerializeField] Text statusText;
    [SerializeField] Text reviewText;
    [SerializeField] LikeToggleController likeToggleController;

    Uranaishi uranaishi;

    public override void OnInstantiate()
    {
        uranaishiButton.onClick.AddListener(OnClickUranaishiButtonButton);
        likeToggleController.OnStart();
    }

    void OnClickUranaishiButtonButton()
    {
        UIManager.i.GetModal<UranaishiModal>().Open(uranaishi);
    }

    public void Show(Review review)
    {
        uranaishi = review.uranaishi;
        likeToggleController.OnOpen(uranaishi, (isOn) =>
        {
            // if (isOn) LikePopup.i.Show(uranaishi);
        });


        iconImage.sprite = null;
        uranaishi.GetIcon((sprite) =>
        {
            iconImage.sprite = sprite;
        });
        nameTxt.text = uranaishi.name;

        reviewText.text = review.text;


        statusText.text = uranaishi.GetStatusDisplayName();
    }


}
