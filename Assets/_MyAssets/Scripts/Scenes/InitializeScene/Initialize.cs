using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InitializeScene
{
    public class Initialize : SingletonMonoBehaviour<Initialize>
    {
        [SerializeField] PopupManager popupManager;
        [SerializeField] Image splashImage;

        [RuntimeInitializeOnLoadMethod]
        static void RuntimeInitializeOnLoad()
        {
            SceneManager.LoadScene(0);
        }

        async void Start()
        {
            popupManager.OnStart();

            await PopupManager.Instance.GetPopup<OnlineCheckPopup>().CheckOnlineUntilOnline();
            DontDestroyOnLoad(gameObject);
            await SceneManager.LoadSceneAsync("Main");
        }

        public void Close()
        {
            splashImage.gameObject.SetActive(false);
        }
    }

}
