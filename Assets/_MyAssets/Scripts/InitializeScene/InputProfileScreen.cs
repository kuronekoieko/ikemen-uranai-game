using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputProfileScreen : SingletonMonoBehaviour<InputProfileScreen>
{


    public void OnStart()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);


    }
}
