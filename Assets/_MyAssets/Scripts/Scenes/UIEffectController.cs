using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectController : MonoBehaviour
{

    [SerializeField] ParticleSystem tapPs;
    [SerializeField] ParticleSystem dragPs;

    [SerializeField] Camera effectCamera;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var screenPos = Input.mousePosition;
            var worldPos = effectCamera.ScreenToWorldPoint(screenPos);
            worldPos.z = 0;
            tapPs.transform.localPosition = worldPos;
            tapPs.Play();
            dragPs.Play();
        }


        if (Input.GetMouseButton(0))
        {
            var screenPos = Input.mousePosition;
            var worldPos = effectCamera.ScreenToWorldPoint(screenPos);
            worldPos.z = 0;
            dragPs.transform.localPosition = worldPos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragPs.Stop();
        }

    }
}
