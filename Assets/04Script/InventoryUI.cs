using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, IBaseTownPopup
{
    [SerializeField]
    private ScrollRect scrollRect;


    List<InventorySlot> slotList = new List<InventorySlot>();
    [SerializeField]
    private GameObject slotPrefab;
    [SerializeField]
    private RectTransform contentTrans;

    private InventorySlot slot;

    private void Awake()
    {
        InitInventory();
    }

    //  ���ʿ� �κ��� ����ϴ� ���Ե� ����
    private void InitInventory()
    {
        for (int i = 0; i < GameManager.Inst.INVEN.MaxSlot; i++)
        {
            slot = Instantiate(slotPrefab, contentTrans).GetComponent<InventorySlot>();
            slot.SLOTINDEX = i;
            slot.gameObject.name = "Slot_" + (i + 1);

            slotList.Add(slot);
        }

        gameObject.LeanScale(Vector3.zero, 0.01f);  //  ������ �� ȭ�鿡 �Ⱥ��̵���
    }


    private List<InventoryItemData> dataList;
    private int currentCount;
    private int maxCount;


    //  �κ��丮�� ���� �� GamaManager�� �����ϴ� �����͸� �����ٰ� UIǥ�⸦ ����
    public void RefreshInventoryUI()
    {
        dataList = GameManager.Inst.INVEN.GetItemList();
        currentCount = GameManager.Inst.INVEN.CurSlot;
        maxCount = GameManager.Inst.INVEN.MaxSlot;

        //Debug.Log(currentCount + " gȮ�� " + maxCount  + "   " + dataList.Count); 

        for (int i = 0; i < maxCount; i++)
        {
            if (i < currentCount && dataList[i].itemTableID > -1)   //  ��ȿ�� ������ �������� üũ, ( ���̺� ID ���󿩺� Ȯ�� )
            {
                slotList[i].DrawItem(dataList[i]);
            }
            else    //  ���������� ���� ���� Ȥ�� �󽽷�
            {
                slotList[i].ClearSlot();
            }
            slotList[i].SelectSlot(false);
        }

    }

    public void PopupOpen()
    {
        RefreshInventoryUI();
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }

    public void PopupClose()
    {
        gameObject.LeanScale(Vector3.zero, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }
}
