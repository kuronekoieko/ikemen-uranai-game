using UnityEngine;
using UnityEditor;
using TMPro;

namespace TMPFontReplacer
{
    public class TMProFontAssetUpdater : EditorWindow
    {
        static TMProFontAssetUpdater tMProFontAssetUpdater;

        [SerializeField] static TMP_FontAsset fontAsset;

        private readonly Vector2 buttonSize = new Vector2(200, 40);

        [MenuItem("Window/TMProFontAssetUpdater")]
        static void Open()
        {
            if (tMProFontAssetUpdater == null)
            {
                tMProFontAssetUpdater = CreateInstance<TMProFontAssetUpdater>();
            }
            tMProFontAssetUpdater.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();

            fontAsset = EditorGUILayout.ObjectField("Font Asset", fontAsset, typeof(TMP_FontAsset), true) as TMP_FontAsset;

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.Space();

                if (GUILayout.Button("Change All Font Assets", GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y)))
                {
                    UpdateAll(fontAsset);
                }

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void UpdateAll(TMP_FontAsset f)
        {
            foreach (TextMeshProUGUI tmp in FindObjectsOfType<TextMeshProUGUI>(true))
            {
                // tmp.font = f;
                // continue;
                if (tmp.font == null)
                {
                    string path = GetPath(tmp.gameObject);
                    Debug.Log(path);
                    Debug.Log("フォント: " + tmp.font.name);
                    continue;
                }
                if (tmp.font.name == "LiberationSans SDF")
                {
                    string path = GetPath(tmp.gameObject);
                    Debug.Log(path);
                    Debug.Log("フォント: " + tmp.font.name);

                    //tmp.font = f;
                }
                if (tmp.font.name == "KiwiMaru-Regular SDF")
                {
                    //tmp.font = f;
                }
            }


        }


        string GetPath(GameObject gameObject)
        {
            string str = gameObject.name;
            if (gameObject.transform.parent)
            {
                str = GetPath(gameObject.transform.parent.gameObject) + "/" + str;
            }
            return str;
        }
    }
}