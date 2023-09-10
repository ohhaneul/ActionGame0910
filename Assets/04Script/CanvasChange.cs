using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasChange : MonoBehaviour
{
    private Canvas canvas;
    private CanvasScaler canvasScaler;

    private void Awake()
    {
        if (!TryGetComponent<Canvas>(out canvas))
            Debug.Log("CanvasChange.cs - Awake() - canvas 참조 실패");
        else
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Camera.main;
            canvas.planeDistance = 1f;
        }

        if (!TryGetComponent<CanvasScaler>(out canvasScaler))
            Debug.Log("CanvasChange.cs - Awake() - CanvasScaler 참조 실패");
        else
        {
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        }
    }
}
