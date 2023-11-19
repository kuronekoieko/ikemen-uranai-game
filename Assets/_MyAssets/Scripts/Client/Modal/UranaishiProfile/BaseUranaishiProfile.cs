using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUranaishiProfile : MonoBehaviour
{
    public abstract void OnStart();
    public abstract void OnOpen(Uranaishi uranaishi);
}
