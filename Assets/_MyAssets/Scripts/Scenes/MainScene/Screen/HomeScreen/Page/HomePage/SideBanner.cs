using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SideBanner : MonoBehaviour
{
    [SerializeField] CustomButton openButton;
    [SerializeField] CustomButton closeButton;
    [SerializeField] Animator animator;

    public async void OnStart()
    {
        openButton.AddListener(async () =>
        {
            await Open(true);
        });
        closeButton.AddListener(async () =>
        {
            await Open(false);
        });

        await Open(false);
    }


    async UniTask Open(bool isOpen)
    {
        closeButton.gameObject.SetActive(isOpen);
        openButton.gameObject.SetActive(!isOpen);

        if (isOpen)
        {
            await animator.PlayAsync("Open");

        }
        else
        {
            await animator.PlayAsync("Close");

        }
    }
}
