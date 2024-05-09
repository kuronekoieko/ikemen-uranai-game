using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPopupButton : MonoBehaviour
{
    [SerializeField] Button button;
    void Start()
    {
        gameObject.SetActive(Application.isEditor);

        button.AddListener(async () =>
        {
            await PopupManager.Instance.GetCommonPopup().ShowAsync(new DataBase.PopupText()
            {
                text = "メンテナンス中です\nしばらく時間をおいてお試しください。",
                button_positive = "OK",
                button_negative = "キャンセル",
            });
        });
    }


}
