using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas), typeof(CanvasScaler))]
public class CanvasManager : SingletonMonoBehaviour<CanvasManager>
{
    public Canvas Canvas
    {
        get
        {
            if (_canvas == null) _canvas = GetComponent<Canvas>();
            return _canvas;
        }
    }
    Canvas _canvas;

    public CanvasScaler CanvasScaler
    {
        get
        {
            if (_canvasScaler == null) _canvasScaler = GetComponent<CanvasScaler>();
            return _canvasScaler;
        }
    }
    CanvasScaler _canvasScaler;
}
