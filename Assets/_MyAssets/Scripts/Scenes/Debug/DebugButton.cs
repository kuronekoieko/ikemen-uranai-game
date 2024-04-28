using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DebugButton : MonoBehaviour
{
    [SerializeField] Button debugButton;
    void Start()
    {
        gameObject.SetActive(Application.isEditor);

        debugButton.AddListener(() =>
        {
            ScreenManager.Instance.Get<DebugScreen>().Open();
            return UniTask.DelayFrame(0);
        });
    }
}
