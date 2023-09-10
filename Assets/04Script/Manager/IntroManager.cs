using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    private GameObject logoObj;

    private void Awake()
    {
        logoObj = GameObject.Find("Logo");

        if (logoObj != null)
        {
            LeanTween.moveLocalY(logoObj, 0f, 3f).setEase(LeanTweenType.easeOutBounce);
            LeanTween.moveLocalX(logoObj, 0f, 3f).setEase(LeanTweenType.easeInSine);
            LeanTween.rotate(logoObj, Vector3.zero, 3f);
            Invoke("AutoNextScene", 3.5f);

        }
        else
            Debug.Log("IntroManager.cs - Awake() - logoObj ���� ����");
    }

    private void AutoNextScene()
    {
        Debug.Log("�������� �̵�");
        //todo : ���� �Ŵ��� �۾� ���� �� ����


    }

}