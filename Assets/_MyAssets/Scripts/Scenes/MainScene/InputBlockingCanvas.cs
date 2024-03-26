using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBlockingCanvas : SingletonMonoBehaviour<InputBlockingCanvas>
{
    private void Awake()
    {
        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
