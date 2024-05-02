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

    public async UniTask Open()
    {
        await splashCG.DOFade(1, 1f).AsyncWaitForCompletion();
    }

    public async UniTask Close()
    {
        await splashCG.DOFade(0, 1f).AsyncWaitForCompletion();
        gameObject.SetActive(false);
    }
}
