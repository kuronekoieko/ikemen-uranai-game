using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling<T> : MonoBehaviour where T : ObjectPoolingElement
{
    [SerializeField] T prefab;
    [SerializeField] protected Transform parent;

    protected List<T> list = new();

    public virtual void OnStart()
    {
        this.prefab.gameObject.SetActive(false);
        HideDummies();
    }

    void HideDummies()
    {
        T[] dummies = parent.GetComponentsInChildren<T>();
        foreach (var dummy in dummies)
        {
            dummy.gameObject.SetActive(false);
        }
    }

    protected void Clear()
    {
        foreach (var item in list)
        {
            item.gameObject.SetActive(false);
        }
    }

    protected T GetInstance()
    {
        T instance = null;
        foreach (var item in list)
        {
            if (item.gameObject.activeSelf == false)
            {
                instance = item;
                break;
            }
        }
        if (instance == null)
        {
            instance = Instantiate(prefab, parent ? parent : transform);
            instance.OnInstantiate();
            list.Add(instance);
        }
        instance.gameObject.SetActive(true);
        return instance;
    }
}
