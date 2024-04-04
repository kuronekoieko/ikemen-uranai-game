using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectController : MonoBehaviour
{

    [SerializeField] ParticleSystem tapPs;
    [SerializeField] ParticleSystem dragPs;
    [SerializeField] Camera effectCamera;

    private void Start()
    {
        var newDragPs = Instantiate(dragPs, tapPs.transform);
        var particleSystems = newDragPs.GetComponentsInChildren<ParticleSystem>();
        foreach (var item in particleSystems)
        {
            ParticleSystem.MainModule mainModule = item.main;
            mainModule.loop = false;
            mainModule.duration = 0.2f;
        }
    }

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
            // https://tsubakit1.hateblo.jp/entry/2017/12/16/154037
            dragPs.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

    }
}
