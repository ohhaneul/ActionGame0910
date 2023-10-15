using System.Collections;
using System.Collections.Generic;  
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class ShopSlot : MonoBehaviour
{
    private Popup_Shop shopPopup;   //  슬롯을 만들어낸 스크립트.

    [SerializeField]
    private Image icon;
    [SerializeField]
    private TextMeshProUGUI itemNameText;
    [SerializeField]
    private TextMeshProUGUI itemPriceText;
    [SerializeField]
    private TextMeshProUGUI sellCountText;

    [SerializeField]
    private Button right;
    [SerializeField]
    private Button left;
    [SerializeField]
    private Button max;

    private InventoryItemData data; //  해당 슬롯 아이템 데이터

    private int slotIndex;
    public int SlotIndex    { get => slotIndex; }

    private int totalGold;
    public int TotalGold    { get => totalGold; }

    private int sellGold;
    public int SellGold { get => sellGold; }

    private int sellMaxCount;
    private int curCount;
    private int itemID;

    private void Awake()
    {
        right.onClick.AddListener(OnClickRightBtn);
        left.onClick.AddListener(OnClickLeftBtn);
        max.onClick.AddListener(OnClickMaxBtn);
    }


    public void OnClickLeftBtn()
    {
        if (curCount > 0)
            curCount--;
        sellCountText.text = curCount.ToString();
        totalGold = sellGold * curCount;
        shopPopup.RefreshGold();
    }
    public void OnClickRightBtn()
    {
        if (sellMaxCount > curCount)
            curCount++;
        sellCountText.text = curCount.ToString();
        totalGold = sellGold * curCount;
        shopPopup.RefreshGold();
    }
    public void OnClickMaxBtn()
    {
        curCount = sellMaxCount;
        sellCountText.text = curCount.ToString();
        totalGold = sellGold * curCount;
        shopPopup.RefreshGold();
    }



    public bool GetSellCount(out int _sellItemID, out int _sellItemCount, out int _sellGold)
    {
        _sellItemID = itemID;
        _sellItemCount = curCount;
        sellMaxCount -= curCount;
        _sellGold = totalGold;
        return true;

    }

    public bool GetBuyCount(out int _buyItemID, out int _buyItemCount, out int _buyGold)
    {
        _buyItemID = itemID;
        _buyItemCount = curCount;
        _buyGold = totalGold;
        return true;

    }

    public void ClearSlot()
    {
        gameObject.SetActive(false);
    }

    public void RefreshSlot(InventoryItemData item)
    {
        gameObject.SetActive(true);
        itemID = item.itemTableID;
        sellMaxCount = item.amount;
        curCount = 0;
        Debug.Log(curCount + " 슬롯의 cur카운트 갱신.");

        GameManager.Inst.GetItemData(itemID, out TableEntity_Item itemData);
        icon.sprite = Resources.Load<Sprite>(itemData.iconImg);
        itemPriceText.text = itemData.sellGold.ToString();
        sellGold = itemData.sellGold;
        sellCountText.text = "0";
        icon.enabled = true;
    }


    public void InitSlot(Popup_Shop _Shop, int index)
    {
        shopPopup = _Shop;
        slotIndex = index;
        icon.enabled = false;
        gameObject.SetActive(false);
    }



}
