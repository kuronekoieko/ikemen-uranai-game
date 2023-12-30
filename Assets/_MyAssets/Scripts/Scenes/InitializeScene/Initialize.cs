using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InitializeScene
{
    public class Initialize : SingletonMonoBehaviour<Initialize>
    {
        async void Start()
        {
            DontDestroyOnLoad(gameObject);
            await SceneManager.LoadSceneAsync("Main");
            // asyncOperation.allowSceneActivation = false;
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }

}
