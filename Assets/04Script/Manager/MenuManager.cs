using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MenuManager : MonoBehaviour
{
    private GameObject inventoryObj;    //  팝업 오브젝트
    private InventoryUI inventoryUI;    //  팝업 스크립트
    private bool isInventoryOpen;       //  인벤토리가 열려있는지 여부


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
                Debug.Log("ManuManager.cs - InitMenuManager () - inventoryUI 참조실패");
            }

            isInventoryOpen = false;
        }

        inventoryBTN = GameObject.Find("InventoryBtn").GetComponent<Button>();
        inventoryBTN.onClick.AddListener(ShowInventory);
    }


    public void ShowInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        if (isInventoryOpen )   //  인벤토리 열기
        {
            inventoryUI.PopupOpen();
        }
        else                    //  인벤토리 닫기
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
