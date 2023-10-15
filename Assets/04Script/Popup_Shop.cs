using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//  C# 클래스 다중상속 허용하지 않음.( 인터페이스는 다중상속이 가능하다.)
public class Popup_Shop : Popup_Base, IBaseTownPopup
{

    [SerializeField]
    private GameObject shopSlotPrefab;  //슬롯 프리펩
    [SerializeField]
    private RectTransform contentRect;  //슬롯 등을 만들어낼 컨텐츠영역
    [SerializeField]
    private RectTransform buyRect;  //  판매 리스트를 생성할 영역
    [SerializeField]
    private TextMeshProUGUI balanceText;    // 잔액 표기 text
    [SerializeField]
    private TextMeshProUGUI amountText;      // 거래 금액 표기 text
    private Inventory inventory;
    private ShopSlot shopSlot;

    List<ShopSlot> sellSlotList = new List<ShopSlot>();
    List<ShopSlot> buySlotList = new List<ShopSlot>();
    List<InventoryItemData> dataList;

    InventoryItemData newItemData = new InventoryItemData();

    [SerializeField]
    private GameObject sellPage;
    [SerializeField]
    private GameObject buyPage;


    private void Awake()
    {
        InitPopup();
        sellTapOpen();  //buytap을 비활성화하기 위해
        PopupClose();
    }
    // ㅇㅇㄹㄴㅇㄹㄴㅇㄹㄴㅇㄹㄴ
    private void InitPopup()
    {
        inventory = GameManager.Inst.INVEN;

        for (int i = 0; i < inventory.MaxSlot; i++)
        {
            shopSlot = Instantiate(shopSlotPrefab, contentRect).GetComponent<ShopSlot>();
            shopSlot.InitSlot(this, i); //슬롯을 생성한 팝업 정보, 몇번째 슬롯인지 index 전달
            shopSlot.gameObject.name = "SellSlot_" + i;
            sellSlotList.Add(shopSlot); // sell 슬롯 관리하는 리스트레 추가.
        }

        for (int i = 0; i < 4; i++) //빨 보 초 파
        {
            shopSlot = Instantiate(shopSlotPrefab, buyRect).GetComponent<ShopSlot>();
            shopSlot.InitSlot(this, i); //슬롯을 생성한 팝업 정보, 몇번째 슬롯인지 index 전달
            shopSlot.gameObject.name = "BuySlot_" + i;
            buySlotList.Add(shopSlot); //buy슬롯 관리하는 리스트에 추가.
        }
    }


    private int totalGold;
    //  유저가 고른 아이템 리스트 (판매 혹은 구매)의 총액을 계산해주는 함수
    public void CalculateGold()
    {
        totalGold = 0;

        if (sellPage.activeSelf)    /// 판매창이 활성화 되었을 때
        {
            for (int i = 0; i < sellSlotList.Count; i++)
            {
                if (sellSlotList[i].isActiveAndEnabled) //  해당 슬롯이 활성화 된 상태라면.
                {
                    totalGold += sellSlotList[i].TotalGold;
                }
            }
        }
        else                        // 구매창이 활성화 되었을 때
        {
            for (int i = 0; i < 4; i++)
            {
                if (buySlotList[i].isActiveAndEnabled)
                {
                    totalGold += buySlotList[i].TotalGold;
                }
            }
        }

    }



    // 팝업에 있는 거래 금액을 갱시해주는 함수
    public void RefreshGold()
    {
        CalculateGold();
        amountText.text = totalGold.ToString();
    }


    // 인벤토리 데이터를 참조해서 판매 목록을 갱신해주는 함수
    public void RefreshData()
    {
        //  플레이어 잔액 갱신
        balanceText.text = GameManager.Inst.PlayerGold.ToString();

        //  인벤토리 정보 불러오기
        inventory = GameManager.Inst.INVEN;

        dataList = inventory.GetItemList(); //  아이템 정보 가졍고

        for (int i = 0; i < inventory.MaxSlot; i++) //  맥스 18
        {
            if (i < inventory.CurSlot && -1 < dataList[i].itemTableID)  // 테이블 정보를 정삭적으로 가지고 있다
            {
                sellSlotList[i].RefreshSlot(dataList[i]);   //  슬롯 정보를 갱신
                Debug.Log("슬롯정보 갱신");
            }
            else
            {
                sellSlotList[i].ClearSlot();    // 빈칸처리
            }
        }
        totalGold = 0;
        amountText.text = "0";
    }


    int itemId, Count, Gold, total; // 거래 ID, 거래 개수, 슬롯의 총액, 거래 총액
    //  유저가 목록(판매 혹은 구매)을 만들고 승인버튼을 눌렀을 때, 실제 거래를 처리하는 함수
    public void ApplyBtn()
    {
        if (sellPage.activeSelf)    // 판매 활성화
        {
            for (int i = inventory.CurSlot -1; i >= 0; i--)
            {
                sellSlotList[i].GetSellCount(out itemId, out Count, out Gold);

                GameManager.Inst.PlayerGold += Gold;    //  플레이어 골드 증가
                InventoryItemData data = new InventoryItemData();
                data.itemTableID = itemId;
                data.amount = Count;
                Debug.Log("삭제 " + Count + "거래 골드" + Gold);
                inventory.DeleteItem(data);
            }
        }
        else    // 구매 활성화
        {
            total = 0;

            for (int i = 0; i < 4; i++)
            {
                buySlotList[i].GetBuyCount(out itemId, out Count, out Gold);
                total += Gold;
            }

            if (total <= GameManager.Inst.PlayerGold)   //  유저가 충분한 골드를 가지고 있을 때만, 거래 성사.
            {
                GameManager.Inst.PlayerGold -= total;
                for (int i = 0; i < 4; i++)
                {
                    if (Count > 0)
                    {
                        InventoryItemData data = new InventoryItemData();
                        data.itemTableID = itemId;
                        data.amount = Count;
                        inventory.AddItem(data);    //  실제 아이템 지금
                    }
                }
            }
            else
            {
                Debug.Log("잔액 부족으로 거래 실패");
            }
        }
        RefreshData();  //  목록을 최신정보로 갱신
    }

    public void sellTapOpen()
    {
        RefreshData();  //  목록을 최신화
        sellPage.SetActive(true);
        buyPage.SetActive(false);
    }
    public void BuyTapOpen()
    {
        RefreshBuyList();   //  구매 목록( 물약 목록을 갱신)
        sellPage.SetActive(false);
        buyPage.SetActive(true);
    }
    private void RefreshBuyList()
    {
        for (int i = 0; i < 4; i++)
        {
            newItemData.itemTableID = 2001001 + i;
            newItemData.amount = 999;
            buySlotList[i].RefreshSlot(newItemData);
        }

    }

    public void PopupClose()
    {
        gameObject.LeanScale(Vector3.zero, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }

    public void PopupOpen()
    {
        RefreshData();  //  팝업창 정보 갱신
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }
}
