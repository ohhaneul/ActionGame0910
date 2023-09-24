using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItemData
{
    public int uid;         //  중복이 안되도록 고유한 Item ID   (  아이템 습득시 GameManager에서 생성
    public int itemTableID; //  테이블 참조를 하기 위한 Table ID
    public int amount;      //  습득한 아이템중에서 중첩이 가능한 아이템의 보유개수 (예 : 물약 90개)
}


[System.Serializable]
public class Inventory
{
    private int maxSlotCount = 18;  //  인벤토리 최대 개수 (아이템 몇가지?)
    public int MaxSlot { get => maxSlotCount; }

    private int curSlotCount;
    public int CurSlot
    {
        get => curSlotCount;
        set => curSlotCount = value;
    }

    [SerializeField]
    private List<InventoryItemData> items = new List<InventoryItemData>();  //  실제 아이템 관리 리스트


    public void AddItem(InventoryItemData newItem)
    {
        int index = FindItemIdex(newItem);

        if (GameManager.Inst.GetItemData(newItem.itemTableID, out TableEntity_Item item))
        {
            if (item.equip) //  장착 가능한 아이템
            {
                newItem.uid = GameManager.Inst.PlayerUidMaker;  //  아이템의 고유 코드 생성
                newItem.amount = 1;
                items.Add(newItem);
                curSlotCount++;
            }
            else if(-1 < index) //  인벤토리 내에 같은 아이템이 있다.
            {
                items[index].amount += newItem.amount;
            }
            else    //  인벤토리 내에 없던 겹치기 가능한 아이템을 습득한 경우
            {
                newItem.uid = GameManager.Inst.PlayerUidMaker;
                items.Add(newItem);
                curSlotCount++;
            }
        }
    }

    //  인벤토리에 여유 공간이 있는지 확인
    public bool isFull()
    {
        return curSlotCount >= maxSlotCount;
    }


    public List<InventoryItemData> GetItemList()
    {
        curSlotCount = items.Count;
        return items;
    }


    //  인벤토리 내에 같은 tableID를 가지고 있는 아이템이 있다면, 몇번째 Index인지 반환
    private int FindItemIdex(InventoryItemData newItem)
    {
        int result = -1;
        for (int i = items.Count -1; i >= 0; i--)
        {
            if (items[i].itemTableID == newItem.itemTableID)
            {
                result = i;
                return result;  //  겹치는 아이템을 찾았다.
            }
        }

        return result;  //  아이템을 찾지 못 해서 -1 리턴
    }

    //잡화 상점에 아이템을 팔았을때 인벤토리 내에서 해당 아이템을 제거하는 함수
    public void DeleteItem(InventoryItemData deleteItem)
    {
        int index = FindItemIdex(deleteItem);   //  인벤토리 내에 아이템이 있는지 확인
        if (-1 < index)
        {
            items[index].amount -= deleteItem.amount;
            if (items[index].amount < 1)
            {
                items.RemoveAt(index);   //  해당 index에 해당하는 요소를 삭제
                curSlotCount--;
            }
        }

    }

    //  강화와 같은 인벤토리 내 보유한 아이템의 정보를 새롭게 변경해야 할 때 호출
    //  newData.uid가 일치하는 아이템이 보유한 item에 있을 경우 데이터를 갱신
    public void UpdateItemInfo(InventoryItemData newData)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].uid == newData.uid)
            {
                items[i].itemTableID = newData.itemTableID;
                items[i].amount = newData.amount;
            }
        }
    }

}
