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

    //  최초에 인벤에 사용하는 슬롯들 생성
    private void InitInventory()
    {
        for (int i = 0; i < GameManager.Inst.INVEN.MaxSlot; i++)
        {
            slot = Instantiate(slotPrefab, contentTrans).GetComponent<InventorySlot>();
            slot.SLOTINDEX = i;
            slot.gameObject.name = "Slot_" + (i + 1);

            slotList.Add(slot);
        }

        gameObject.LeanScale(Vector3.zero, 0.01f);  //  시작할 때 화면에 안보이도록
    }


    private List<InventoryItemData> dataList;
    private int currentCount;
    private int maxCount;


    //  인벤토리가 열릴 때 GamaManager가 관리하는 데이터를 가져다가 UI표기를 갱신
    public void RefreshInventoryUI()
    {
        dataList = GameManager.Inst.INVEN.GetItemList();
        currentCount = GameManager.Inst.INVEN.CurSlot;
        maxCount = GameManager.Inst.INVEN.MaxSlot;

        //Debug.Log(currentCount + " g확인 " + maxCount  + "   " + dataList.Count); 

        for (int i = 0; i < maxCount; i++)
        {
            if (i < currentCount && dataList[i].itemTableID > -1)   //  유효한 아이템 정보인지 체크, ( 테이블 ID 정상여부 확인 )
            {
                slotList[i].DrawItem(dataList[i]);
            }
            else    //  정상적이지 않은 슬롯 혹은 빈슬롯
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
