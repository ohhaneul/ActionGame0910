using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItemData
{
    public int uid;         //  �ߺ��� �ȵǵ��� ������ Item ID   (  ������ ����� GameManager���� ����
    public int itemTableID; //  ���̺� ������ �ϱ� ���� Table ID
    public int amount;      //  ������ �������߿��� ��ø�� ������ �������� �������� (�� : ���� 90��)
}


[System.Serializable]
public class Inventory
{
    private int maxSlotCount = 18;  //  �κ��丮 �ִ� ���� (������ ���?)
    public int MaxSlot { get => maxSlotCount; }

    private int curSlotCount;
    public int CurSlot
    {
        get => curSlotCount;
        set => curSlotCount = value;
    }

    [SerializeField]
    private List<InventoryItemData> items = new List<InventoryItemData>();  //  ���� ������ ���� ����Ʈ


    public void AddItem(InventoryItemData newItem)
    {
        int index = FindItemIdex(newItem);

        if (GameManager.Inst.GetItemData(newItem.itemTableID, out TableEntity_Item item))
        {
            if (item.equip) //  ���� ������ ������
            {
                newItem.uid = GameManager.Inst.PlayerUidMaker;  //  �������� ���� �ڵ� ����
                newItem.amount = 1;
                items.Add(newItem);
                curSlotCount++;
            }
            else if(-1 < index) //  �κ��丮 ���� ���� �������� �ִ�.
            {
                items[index].amount += newItem.amount;
            }
            else    //  �κ��丮 ���� ���� ��ġ�� ������ �������� ������ ���
            {
                newItem.uid = GameManager.Inst.PlayerUidMaker;
                items.Add(newItem);
                curSlotCount++;
            }
        }
    }

    //  �κ��丮�� ���� ������ �ִ��� Ȯ��
    public bool isFull()
    {
        return curSlotCount >= maxSlotCount;
    }


    public List<InventoryItemData> GetItemList()
    {
        curSlotCount = items.Count;
        return items;
    }


    //  �κ��丮 ���� ���� tableID�� ������ �ִ� �������� �ִٸ�, ���° Index���� ��ȯ
    private int FindItemIdex(InventoryItemData newItem)
    {
        int result = -1;
        for (int i = items.Count -1; i >= 0; i--)
        {
            if (items[i].itemTableID == newItem.itemTableID)
            {
                result = i;
                return result;  //  ��ġ�� �������� ã�Ҵ�.
            }
        }

        return result;  //  �������� ã�� �� �ؼ� -1 ����
    }

    //��ȭ ������ �������� �Ⱦ����� �κ��丮 ������ �ش� �������� �����ϴ� �Լ�
    public void DeleteItem(InventoryItemData deleteItem)
    {
        int index = FindItemIdex(deleteItem);   //  �κ��丮 ���� �������� �ִ��� Ȯ��
        if (-1 < index)
        {
            items[index].amount -= deleteItem.amount;
            if (items[index].amount < 1)
            {
                items.RemoveAt(index);   //  �ش� index�� �ش��ϴ� ��Ҹ� ����
                curSlotCount--;
            }
        }

    }

    //  ��ȭ�� ���� �κ��丮 �� ������ �������� ������ ���Ӱ� �����ؾ� �� �� ȣ��
    //  newData.uid�� ��ġ�ϴ� �������� ������ item�� ���� ��� �����͸� ����
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
