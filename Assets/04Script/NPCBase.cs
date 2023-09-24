using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  충돌 collider
//  충돌 이벤트 검툴 rigbody
[RequireComponent(typeof(SphereCollider))]  //  스크립트를 가지고 있는 게임 오브젝트가
[RequireComponent(typeof(Rigidbody))]  //  해당 컴포넌트가 없을 경우자동으로 추가


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
            Debug.Log("NPCBase.cs - InitNPC() - col 참조실패");
        else
        {
            col.isTrigger = true;
            col.radius = 2.5f;
        }

        if (!TryGetComponent<Rigidbody>(out rig))
            Debug.Log("NPCBase.cs - InitNPC() - rig 참조실패");
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
            Debug.Log("팝업 열기. ");
            isOn = true;
            targetPopup.GetComponent<IBaseTownPopup>().PopupOpen();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isOn && other.CompareTag("Player"))
        {
            Debug.Log("팝업 닫기. ");

            isOn = false;
            targetPopup.GetComponent<IBaseTownPopup>().PopupClose();
        }
    }
}
