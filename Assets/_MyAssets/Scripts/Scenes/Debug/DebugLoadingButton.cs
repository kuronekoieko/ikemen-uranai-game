using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLoadingButton : MonoBehaviour
{

    [SerializeField] Button button;
    void Start()
    {
        gameObject.SetActive(Application.isEditor);

        button.AddListener(async () =>
        {
            ScreenManager.Instance.Get<LoadingScreen>().Open();
            await ScreenManager.Instance.Get<LoadingScreen>().ProgressTimer(5f);
            await ScreenManager.Instance.Get<LoadingScreen>().Close();
        });
    }
}
