using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UranaishiListModal : BaseModal
{
    [SerializeField] UranaishiButtonManager uranaishiButtonManager;

    public override void OnStart()
    {
        base.OnStart();
        uranaishiButtonManager.OnStart();

        uranaishiButtonManager.ShowButtons(UIManager.i.uranaishiAry.Take(20).ToArray());
    }

    public void Open()
    {
        base.OpenAnim(ModalType.Horizontal);
    }
}
