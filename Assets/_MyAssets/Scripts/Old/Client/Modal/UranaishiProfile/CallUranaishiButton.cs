using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CallUranaishiButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Text text;
    [SerializeField] CallingManager callingManager;


    Uranaishi uranaishi;

    public void OnStart()
    {
        callingManager.OnStart();
        button.onClick.AddListener(OnClick);
    }




    public void OnOpen(Uranaishi uranaishi)
    {
        this.uranaishi = uranaishi;
        text.text = $"今すぐ予約する\n({uranaishi.GetChareText()})";

        // addlistenerだと、登録したメソッドを初期化したりしないといけない
        switch (uranaishi.status)
        {
            case UranaishiStatus.Counseling:
                break;
            case UranaishiStatus.Waiting:
                text.text = $"電話で占う\n({uranaishi.GetChareText()})";
                break;
            case UranaishiStatus.Closed:
                break;
            case UranaishiStatus.DatTime:
                break;
            default:
                break;
        }
    }

    void OnClick()
    {
        switch (uranaishi.status)
        {
            case UranaishiStatus.Counseling:
                break;
            case UranaishiStatus.Waiting:
                callingManager.Open(uranaishi);
                break;
            case UranaishiStatus.Closed:
                break;
            case UranaishiStatus.DatTime:
                break;
            default:
                break;
        }
    }
}
