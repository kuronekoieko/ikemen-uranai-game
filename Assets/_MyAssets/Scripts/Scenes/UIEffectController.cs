using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectController : MonoBehaviour
{

    [SerializeField] ParticleSystem tapPs;
    [SerializeField] Camera uiCamera;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var screenPos = Input.mousePosition;
            var worldPos = uiCamera.ScreenToWorldPoint(screenPos);
            worldPos.z = 0;
            tapPs.transform.localPosition = worldPos;
            tapPs.Play();
        }
    }
}
