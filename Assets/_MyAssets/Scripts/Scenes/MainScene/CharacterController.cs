using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider2D))]
public class CharacterController : MonoBehaviour, IPointerClickHandler
{
    void Start()
    {
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("IPointerClickHandler: " + gameObject.name);

    }
}
