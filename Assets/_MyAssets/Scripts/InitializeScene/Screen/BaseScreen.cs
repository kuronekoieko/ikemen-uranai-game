using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScreen : MonoBehaviour
{
    public virtual void OnStart()
    {
        gameObject.SetActive(false);
    }
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
}
