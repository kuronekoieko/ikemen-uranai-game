using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCallingScreen : MonoBehaviour
{
    protected CallingManager manager;
    protected Uranaishi uranaishi;

    public virtual void OnStart(CallingManager manager)
    {
        this.manager = manager;
        Close();

    }

    public virtual void Open(Uranaishi uranaishi)
    {
        this.uranaishi = uranaishi;
        gameObject.SetActive(true);

    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

}