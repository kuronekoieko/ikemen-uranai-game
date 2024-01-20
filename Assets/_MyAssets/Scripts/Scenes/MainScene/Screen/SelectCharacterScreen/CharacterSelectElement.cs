using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterSelectElement : ObjectPoolingElement
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI cvText;
    public TextMeshProUGUI descriptionText;
    public Image characterImage;

    public override void OnInstantiate()
    {
    }

    public async void Show(DataBase.Character character)
    {
        nameText.text = character.name_jp;
        cvText.text = "CV: " + character.voice_actor_jp;
        descriptionText.text = character.description;

        string address = AssetBundleLoader.GetCharacterFullAddress(character.id);
        Sprite sprite = await AssetBundleLoader.LoadAssetAsync<Sprite>(address);
        if (sprite == null) return;
        characterImage.sprite = sprite;
    }
}
