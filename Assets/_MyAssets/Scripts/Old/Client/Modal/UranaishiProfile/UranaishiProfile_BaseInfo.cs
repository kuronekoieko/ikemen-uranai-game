using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UranaishiProfile_BaseInfo : BaseUranaishiProfile
{
    [SerializeField] TitleContentTexts titleContentTextOrigin;
    List<TitleContentTexts> titleContentTexts = new List<TitleContentTexts>();

    public override void OnStart()
    {
        titleContentTexts.Add(titleContentTextOrigin);
        for (int i = 0; i < 4; i++)
        {
            titleContentTexts.Add(Instantiate(titleContentTextOrigin, transform));
        }

    }

    public override void OnOpen(Uranaishi uranaishi)
    {
        for (int i = 0; i < titleContentTexts.Count; i++)
        {
            titleContentTexts[i].gameObject.SetActive(false);
        }
        titleContentTexts[0].SetTexts("得意分野", uranaishi.expertises.GetJoinedKeywords());
        titleContentTexts[1].SetTexts("占術", uranaishi.divinations.GetJoinedKeywords());
    }
}
