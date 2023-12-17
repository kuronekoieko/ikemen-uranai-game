using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BasePage : MonoBehaviour
{
    public virtual void OnStart()
    {
        gameObject.SetActive(false);
    }
    public virtual async void Open()
    {
        var basePage = PageManager.Instance.GetActive();
        basePage?.Close();
        await UniTask.DelayFrame(1);
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {

        gameObject.SetActive(false);
        Debug.Log("親");
    }
}
