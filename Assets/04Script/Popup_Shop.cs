using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//  C# Ŭ���� ���߻�� ������� ����.( �������̽��� ���߻���� �����ϴ�.)
public class Popup_Shop : Popup_Base, IBaseTownPopup
{

    [SerializeField]
    private GameObject shopSlotPrefab;  //���� ������
    [SerializeField]
    private RectTransform contentRect;  //���� ���� ���� ����������
    [SerializeField]
    private RectTransform buyRect;  //  �Ǹ� ����Ʈ�� ������ ����
    [SerializeField]
    private TextMeshProUGUI balanceText;    // �ܾ� ǥ�� text
    [SerializeField]
    private TextMeshProUGUI amountText;      // �ŷ� �ݾ� ǥ�� text
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
        sellTapOpen();  //buytap�� ��Ȱ��ȭ�ϱ� ����
        PopupClose();
    }
    // ��������������������������
    private void InitPopup()
    {
        inventory = GameManager.Inst.INVEN;

        for (int i = 0; i < inventory.MaxSlot; i++)
        {
            shopSlot = Instantiate(shopSlotPrefab, contentRect).GetComponent<ShopSlot>();
            shopSlot.InitSlot(this, i); //������ ������ �˾� ����, ���° �������� index ����
            shopSlot.gameObject.name = "SellSlot_" + i;
            sellSlotList.Add(shopSlot); // sell ���� �����ϴ� ����Ʈ�� �߰�.
        }

        for (int i = 0; i < 4; i++) //�� �� �� ��
        {
            shopSlot = Instantiate(shopSlotPrefab, buyRect).GetComponent<ShopSlot>();
            shopSlot.InitSlot(this, i); //������ ������ �˾� ����, ���° �������� index ����
            shopSlot.gameObject.name = "BuySlot_" + i;
            buySlotList.Add(shopSlot); //buy���� �����ϴ� ����Ʈ�� �߰�.
        }
    }


    private int totalGold;
    //  ������ �� ������ ����Ʈ (�Ǹ� Ȥ�� ����)�� �Ѿ��� ������ִ� �Լ�
    public void CalculateGold()
    {
        totalGold = 0;

        if (sellPage.activeSelf)    /// �Ǹ�â�� Ȱ��ȭ �Ǿ��� ��
        {
            for (int i = 0; i < sellSlotList.Count; i++)
            {
                if (sellSlotList[i].isActiveAndEnabled) //  �ش� ������ Ȱ��ȭ �� ���¶��.
                {
                    totalGold += sellSlotList[i].TotalGold;
                }
            }
        }
        else                        // ����â�� Ȱ��ȭ �Ǿ��� ��
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



    // �˾��� �ִ� �ŷ� �ݾ��� �������ִ� �Լ�
    public void RefreshGold()
    {
        CalculateGold();
        amountText.text = totalGold.ToString();
    }


    // �κ��丮 �����͸� �����ؼ� �Ǹ� ����� �������ִ� �Լ�
    public void RefreshData()
    {
        //  �÷��̾� �ܾ� ����
        balanceText.text = GameManager.Inst.PlayerGold.ToString();

        //  �κ��丮 ���� �ҷ�����
        inventory = GameManager.Inst.INVEN;

        dataList = inventory.GetItemList(); //  ������ ���� ������

        for (int i = 0; i < inventory.MaxSlot; i++) //  �ƽ� 18
        {
            if (i < inventory.CurSlot && -1 < dataList[i].itemTableID)  // ���̺� ������ ���������� ������ �ִ�
            {
                sellSlotList[i].RefreshSlot(dataList[i]);   //  ���� ������ ����
                Debug.Log("�������� ����");
            }
            else
            {
                sellSlotList[i].ClearSlot();    // ��ĭó��
            }
        }
        totalGold = 0;
        amountText.text = "0";
    }


    int itemId, Count, Gold, total; // �ŷ� ID, �ŷ� ����, ������ �Ѿ�, �ŷ� �Ѿ�
    //  ������ ���(�Ǹ� Ȥ�� ����)�� ����� ���ι�ư�� ������ ��, ���� �ŷ��� ó���ϴ� �Լ�
    public void ApplyBtn()
    {
        if (sellPage.activeSelf)    // �Ǹ� Ȱ��ȭ
        {
            for (int i = inventory.CurSlot -1; i >= 0; i--)
            {
                sellSlotList[i].GetSellCount(out itemId, out Count, out Gold);

                GameManager.Inst.PlayerGold += Gold;    //  �÷��̾� ��� ����
                InventoryItemData data = new InventoryItemData();
                data.itemTableID = itemId;
                data.amount = Count;
                Debug.Log("���� " + Count + "�ŷ� ���" + Gold);
                inventory.DeleteItem(data);
            }
        }
        else    // ���� Ȱ��ȭ
        {
            total = 0;

            for (int i = 0; i < 4; i++)
            {
                buySlotList[i].GetBuyCount(out itemId, out Count, out Gold);
                total += Gold;
            }

            if (total <= GameManager.Inst.PlayerGold)   //  ������ ����� ��带 ������ ���� ����, �ŷ� ����.
            {
                GameManager.Inst.PlayerGold -= total;
                for (int i = 0; i < 4; i++)
                {
                    if (Count > 0)
                    {
                        InventoryItemData data = new InventoryItemData();
                        data.itemTableID = itemId;
                        data.amount = Count;
                        inventory.AddItem(data);    //  ���� ������ ����
                    }
                }
            }
            else
            {
                Debug.Log("�ܾ� �������� �ŷ� ����");
            }
        }
        RefreshData();  //  ����� �ֽ������� ����
    }

    public void sellTapOpen()
    {
        RefreshData();  //  ����� �ֽ�ȭ
        sellPage.SetActive(true);
        buyPage.SetActive(false);
    }
    public void BuyTapOpen()
    {
        RefreshBuyList();   //  ���� ���( ���� ����� ����)
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
        RefreshData();  //  �˾�â ���� ����
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }
}
