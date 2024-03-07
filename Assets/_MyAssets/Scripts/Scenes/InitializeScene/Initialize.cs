using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InitializeScene
{
    public class Initialize : SingletonMonoBehaviour<Initialize>
    {
        [SerializeField] PopupManager popupManager;

        async void Start()
        {
            popupManager.OnStart();
            // Debug.Log("入力待ち");
            // await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            bool isOnline = await PopupManager.Instance.GetPopup<OnlineCheckPopup>().CheckOnline();

            while (isOnline == false)
            {
                isOnline = await PopupManager.Instance.GetPopup<OnlineCheckPopup>().CheckOnline();
            }

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
