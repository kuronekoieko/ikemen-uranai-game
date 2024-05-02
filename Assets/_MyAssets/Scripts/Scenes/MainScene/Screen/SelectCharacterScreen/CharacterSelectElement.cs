using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterSelectElement : ObjectPoolingElement
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI cvText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] Image characterImage;

    public override void OnInstantiate()
    {
    }

    public async void Show(DataBase.Character character)
    {
        nameText.text = character.name_jp;
        cvText.text = "CV: " + character.voice_actor_jp;
        descriptionText.text = character.description;

        string address = AddressablesLoader.GetCharacterFullAddress(character.id);
        Sprite sprite = await AddressablesLoader.LoadAsync<Sprite>(address);
        if (sprite == null) return;
        characterImage.sprite = sprite;
    }
}
