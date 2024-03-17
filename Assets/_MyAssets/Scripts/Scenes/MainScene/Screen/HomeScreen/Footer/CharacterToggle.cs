using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterToggle : BaseFooterToggleController
{
    public override void OnStart()
    {
        base.OnStart();
        base.SetSelectedAction(Character);
    }

    public void Character()
    {
        PageManager.Instance.Get<CharactersPage>().Open();
    }
}
