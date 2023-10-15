using System.Collections;
using System.Collections.Generic;
using UnityEngine;  
using UnityEngine.UI;
using TMPro;

public class Popup_Forge : Popup_Base, IBaseTownPopup
{
    [SerializeField]
    private GameObject forgeSlotFrefab;
    [SerializeField]
    private RectTransform contentRect;

    [SerializeField]
    private Image iconImg;
    [SerializeField]
    private TextMeshProUGUI balanceText;
    [SerializeField]
    private TextMeshProUGUI enchantPrice;
    [SerializeField]
    private TextMeshProUGUI enchantLevel;
    [SerializeField]
    private TextMeshProUGUI enchantAfterLevel;

    [SerializeField]
    private Button applyBtn;

    private List<ForgeSlot> forgeSlotList = new List<ForgeSlot>();
    private Inventory inventory;


    private void Awake()
    {
        InitPopup();
        PopupClose();
    }

    public void InitPopup()
    {
        inventory = GameManager.Inst.INVEN;

        for (int i = 0; i < inventory.MaxSlot; i++)
        {
            ForgeSlot slot = Instantiate(forgeSlotFrefab, contentRect).GetComponent<ForgeSlot>();
            slot.gameObject.name = "ForgeSlot_" + i;
            slot.CreateSlot(this);
            forgeSlotList.Add(slot);
        }

        applyBtn.onClick.AddListener(OnClick_Enchant);

    }

    private InventoryItemData selectItem;   //  선택 슬롯 아이템 정보
    private List<InventoryItemData> dataList;   //  인벤에 있는 아이템들 중 강화가 가능한 아이템 리스트만 뽑아서 관리
    private TableEntity_Item tableInfo;


    public void SelectItem(InventoryItemData itemData)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].uid == itemData.uid)
            {
                selectItem = itemData;
                if (GameManager.Inst.GetItemData(itemData.itemTableID, out TableEntity_Item tableData)) //  선택된 아이템의 테이블 정보
                {
                    iconImg.enabled = true;
                    iconImg.sprite = Resources.Load<Sprite>(tableData.iconImg);
                }
                else
                    iconImg.enabled = false;

                enchantPrice.text = (itemData.itemTableID % 1000 * 500).ToString(); // 강화에 필요한 금액
                enchantLevel.text = (itemData.itemTableID % 1000).ToString();  //  현재 강화도 표시
                enchantAfterLevel.text = (itemData.itemTableID % 1000 + 1).ToString();  //  강화 성공했을 때의 강화도 푯
            }
            else
                forgeSlotList[i].SelectFocus(false);

        }
    }
    private void RefreshData()
    {
        balanceText.text = GameManager.Inst.PlayerGold.ToString();  // 플레이어 골드 표시

        inventory = GameManager.Inst.INVEN; //   인벤 받아오기


        dataList = inventory.GetItemList();//.ToList<InventoryItemData>(); //  깊은 복사. 각각희 포인터가 각각의 메모리를 가리키고 대상의 메모리값 복사


        for (int i = inventory.CurSlot -1; i >= 0; i--)
        {
            if (GameManager.Inst.GetItemData(dataList[i].itemTableID, out TableEntity_Item itemData) // xpdlqmf wjdqh ckawh tjdrhd
                && itemData.equip)     // 장착 불가능한 아이템
            {
                dataList.RemoveAt(i);   //  removeAt() 인덱스를 기준으로 리스트에서 삭제
            }
        }

        for (int i = 0;  i < forgeSlotList.Count; i++)
        {
            if (i < dataList.Count)    //  표기해야 하는 아이템 슬롯
            {
                forgeSlotList[i].RefreshSlot(dataList[i]);
            }
            else    // 표기할 아이템 없음
            {
                forgeSlotList[i].ClearSlot();
            }
        }
    }

    public void OnClick_Enchant()   // qjxms snffuTdmfEo rkdghk tleh
    {
        if (TryEnchant())   // 강화시도 한 뒤에 성공
        {
            selectItem.itemTableID++;
            GameManager.Inst.INVEN.UpdateItemInfo(selectItem);
            SelectItem(selectItem);
        }
        else    // 실패
        {

        }
    }

    private bool TryEnchant()   //  강화 시도를 처리하는 함수
    {
        bool isSuccess = false;
        if (CanEnchant())
        {
            isSuccess = Random.Range(0, 10001) < 9000; // 90% 성공확률
            GameManager.Inst.PlayerGold -= ((selectItem.itemTableID % 1000) * 500);
            RefreshData();
        }
        return false;
    }
    private bool CanEnchant()   //  강화 시도를 하기 위한 조건이 충족 되는지 확인 하는 함수.
    {
        if(selectItem.itemTableID % 1000 >= 5)  //  최대 강화 상태인가?
        {
            Debug.Log("맥스 강화 상태");
            return false;
        }

        if (selectItem.itemTableID % 1000 * 500 > GameManager.Inst.PlayerGold)  //  강화 비용 부족
        {
            Debug.Log("강화 시도에 필요한 골드가 부족합니다.");
            return false;
        }

        return false;   //  강화를 시도해 볼 수 있다.
    }



    public void PopupClose()
    {
        gameObject.LeanScale(Vector3.zero, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }

    public void PopupOpen()
    {
        RefreshData();
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }
}
