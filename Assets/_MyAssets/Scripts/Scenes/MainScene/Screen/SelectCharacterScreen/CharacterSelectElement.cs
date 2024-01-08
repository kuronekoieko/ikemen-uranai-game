using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterSelectElement : ObjectPoolingElement
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI cvText;
    public TextMeshProUGUI descriptionText;

    public override void OnInstantiate()
    {
    }

    public void Show(DataBase.Character character)
    {
        nameText.text = character.name_jp;
        cvText.text = "CV: " + character.voice_actor_jp;
        descriptionText.text = character.description;
    }
}
