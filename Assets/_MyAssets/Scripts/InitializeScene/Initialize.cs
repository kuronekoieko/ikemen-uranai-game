using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class Initialize : MonoBehaviour
{
    async void Start()
    {
        LoadingScreen.Instance.OnStart();
        LoadingScreen.Instance.Open();
        await SceneManager.LoadSceneAsync("Main");
    }


}
