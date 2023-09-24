using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MenuManager : MonoBehaviour
{
    private GameObject inventoryObj;    //  �˾� ������Ʈ
    private InventoryUI inventoryUI;    //  �˾� ��ũ��Ʈ
    private bool isInventoryOpen;       //  �κ��丮�� �����ִ��� ����


    private Button inventoryBTN;

    private void Awake()
    {
        InitMenuManager();
    }

    public void InitMenuManager()
    {
        if (inventoryObj == null)
        {
            inventoryObj = GameObject.Find("Inventory");
            if (inventoryObj && !inventoryObj.TryGetComponent<InventoryUI>(out inventoryUI))
            {
                Debug.Log("ManuManager.cs - InitMenuManager () - inventoryUI ��������");
            }

            isInventoryOpen = false;
        }

        inventoryBTN = GameObject.Find("InventoryBtn").GetComponent<Button>();
        inventoryBTN.onClick.AddListener(ShowInventory);
    }


    public void ShowInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        if (isInventoryOpen )   //  �κ��丮 ����
        {
            inventoryUI.PopupOpen();
        }
        else                    //  �κ��丮 �ݱ�
        {
            inventoryUI.PopupClose();
        }
    }

    public void TestBtnSave()
    {
        GameManager.Inst.SaveData();
    }
    public void TestBtnLoad()
    {
        GameManager.Inst.LoadData();
    }
}
