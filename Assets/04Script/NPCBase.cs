using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  �浹 collider
//  �浹 �̺�Ʈ ���� rigbody
[RequireComponent(typeof(SphereCollider))]  //  ��ũ��Ʈ�� ������ �ִ� ���� ������Ʈ��
[RequireComponent(typeof(Rigidbody))]  //  �ش� ������Ʈ�� ���� ����ڵ����� �߰�


public class NPCBase : MonoBehaviour
{
    private SphereCollider col;
    private Rigidbody rig;

    [SerializeField]
    private GameObject targetPopup;

    private bool isOn;

    private void Awake()
    {
        InitNPC();
    }

    private void InitNPC()
    {
        if (! TryGetComponent<SphereCollider>(out col))
            Debug.Log("NPCBase.cs - InitNPC() - col ��������");
        else
        {
            col.isTrigger = true;
            col.radius = 2.5f;
        }

        if (!TryGetComponent<Rigidbody>(out rig))
            Debug.Log("NPCBase.cs - InitNPC() - rig ��������");
        else
        {
            rig.useGravity = false;
            rig.isKinematic = true;
        }

        isOn = false;
    }




    private void OnTriggerEnter(Collider other)
    {
        if (!isOn && other.CompareTag("Player"))
        {
            Debug.Log("�˾� ����. ");
            isOn = true;
            targetPopup.GetComponent<IBaseTownPopup>().PopupOpen();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isOn && other.CompareTag("Player"))
        {
            Debug.Log("�˾� �ݱ�. ");

            isOn = false;
            targetPopup.GetComponent<IBaseTownPopup>().PopupClose();
        }
    }
}
