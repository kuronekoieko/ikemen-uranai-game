using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class SplashCanvas : MonoBehaviour
{
    [SerializeField] CanvasGroup splashCG;

    private void Awake()
    {
        splashCG.alpha = 0;
    }

    public void Open()
    {
        splashCG.DOFade(1, 1f);
    }

    public async UniTask Close()
    {
        await splashCG.DOFade(0, 1f).AsyncWaitForCompletion();
        gameObject.SetActive(false);
    }
}
