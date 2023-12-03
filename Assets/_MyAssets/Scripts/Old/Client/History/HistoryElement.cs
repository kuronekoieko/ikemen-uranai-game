using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class HistoryElement : ObjectPoolingElement
{
    [SerializeField] Button uranaishiButton;
    [SerializeField] Image iconImage;
    [SerializeField] Text nameTxt;
    [SerializeField] Text durationText;
    [SerializeField] Text dateTimeText;
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

    public void Show(History history)
    {

        uranaishi = UIManager.i.uranaishiAry
        .FirstOrDefault(uranaishi => uranaishi.id == history.uranaishiId);

        likeToggleController.OnOpen(uranaishi, (isOn) =>
        {
            if (isOn) LikePopup.i.Show(uranaishi);
        });

        iconImage.sprite = null;
        uranaishi.GetIcon((sprite) =>
        {
            iconImage.sprite = sprite;
        });
        nameTxt.text = uranaishi.name;

        var leftTimeSpan = new TimeSpan(0, 0, history.durationSec);
        var str = leftTimeSpan.ToString(@"h'時間'm'分'");

        durationText.text = str;
        dateTimeText.text = history.startCallingSDT.dateTime.ToStringIncludeEmpty("yyyy/MM/dd H:m");
    }

}
