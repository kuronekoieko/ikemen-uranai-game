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
            await PopupManager.Instance.GetCommonPopup().ShowAsync(
        "",
        "メンテナンス中です\nしばらく時間をおいてお試しください。",
        "OK",
        "キャンセル"
        );

        });
    }


}
