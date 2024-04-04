using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SideBanner : MonoBehaviour
{
    [SerializeField] Button openButton;
    [SerializeField] Button closeButton;
    [SerializeField] Animator animator;

    public void OnStart()
    {
        openButton.AddListener(async () =>
        {
            await Open(true);
        });
        closeButton.AddListener(async () =>
        {
            await Open(false);
        });
        // https://tsubakit1.hateblo.jp/entry/2018/10/04/233000
        // gameobjectが最アクティブになると、アニメーションがentryに戻るので、オフにする
        // ページ切替時に動いてしまうため
        animator.keepAnimatorStateOnDisable = true;
    }

    public void OnOpen()
    {
    }


    async UniTask Open(bool isOpen)
    {
        closeButton.gameObject.SetActive(isOpen);
        openButton.gameObject.SetActive(!isOpen);

        if (isOpen)
        {
            await animator.SetTriggerAsync("Open");
        }
        else
        {
            await animator.SetTriggerAsync("Close");
        }
    }
}
