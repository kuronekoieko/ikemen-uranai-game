using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class Initialize : MonoBehaviour
{
    async void Start()
    {
        await SceneManager.LoadSceneAsync("Main");
    }


}
