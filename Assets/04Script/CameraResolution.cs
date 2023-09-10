using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private void Awake()
    {
        if (!TryGetComponent<Camera>(out Camera cam))
            Debug.Log("CameraResolution.cs - Awake() - cam 참조 실패");

        Rect rt = cam.rect;
        float scale_Height = ((float)Screen.width / Screen.height) / ((float)16f / 9f);  //해상도 지정
        float scale_Width = 1f / scale_Height;

        if (scale_Height < 1)
        {
            rt.height = scale_Height;
            rt.y = (1f - scale_Height) / 2f;
        }
        else
        {
            rt.width = scale_Width;
            rt.x = (1f - scale_Width) / 2f;
        }
        cam.rect = rt;
    }
}
