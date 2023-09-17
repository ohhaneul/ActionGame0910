using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;

    [SerializeField]
    private Vector3 offset;

    private void Awake()
    {
        InitFollowCamera();
    }

    private bool InitFollowCamera()
    {
        target = GameObject.Find("MainPlayer").transform;

        return target != null;
    }

    private void Update()
    {
        if (target == null)
        {
            if (!InitFollowCamera())
                Debug.Log("CameraFollow.cs - update() - target is null");
        }
        else
            transform.position = target.position + offset;
    }
        

}
