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

            await PopupManager.Instance.GetPopup<OnlineCheckPopup>().CheckOnlineUntilOnline();
            DontDestroyOnLoad(gameObject);
            await SceneManager.LoadSceneAsync("Main");
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }

}
