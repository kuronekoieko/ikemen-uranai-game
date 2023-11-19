using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ReadOnlyValue<T>
{
    [SerializeField] T _value;
    public T value => _value;
}
