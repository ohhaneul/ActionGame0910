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

    private InventoryItemData selectItem;   //  ���� ���� ������ ����
    private List<InventoryItemData> dataList;   //  �κ��� �ִ� �����۵� �� ��ȭ�� ������ ������ ����Ʈ�� �̾Ƽ� ����
    private TableEntity_Item tableInfo;


    public void SelectItem(InventoryItemData itemData)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].uid == itemData.uid)
            {
                selectItem = itemData;
                if (GameManager.Inst.GetItemData(itemData.itemTableID, out TableEntity_Item tableData)) //  ���õ� �������� ���̺� ����
                {
                    iconImg.enabled = true;
                    iconImg.sprite = Resources.Load<Sprite>(tableData.iconImg);
                }
                else
                    iconImg.enabled = false;

                enchantPrice.text = (itemData.itemTableID % 1000 * 500).ToString(); // ��ȭ�� �ʿ��� �ݾ�
                enchantLevel.text = (itemData.itemTableID % 1000).ToString();  //  ���� ��ȭ�� ǥ��
                enchantAfterLevel.text = (itemData.itemTableID % 1000 + 1).ToString();  //  ��ȭ �������� ���� ��ȭ�� ǩ
            }
            else
                forgeSlotList[i].SelectFocus(false);

        }
    }
    private void RefreshData()
    {
        balanceText.text = GameManager.Inst.PlayerGold.ToString();  // �÷��̾� ��� ǥ��

        inventory = GameManager.Inst.INVEN; //   �κ� �޾ƿ���


        dataList = inventory.GetItemList();//.ToList<InventoryItemData>(); //  ���� ����. ������ �����Ͱ� ������ �޸𸮸� ����Ű�� ����� �޸𸮰� ����


        for (int i = inventory.CurSlot -1; i >= 0; i--)
        {
            if (GameManager.Inst.GetItemData(dataList[i].itemTableID, out TableEntity_Item itemData) // xpdlqmf wjdqh ckawh tjdrhd
                && itemData.equip)     // ���� �Ұ����� ������
            {
                dataList.RemoveAt(i);   //  removeAt() �ε����� �������� ����Ʈ���� ����
            }
        }

        for (int i = 0;  i < forgeSlotList.Count; i++)
        {
            if (i < dataList.Count)    //  ǥ���ؾ� �ϴ� ������ ����
            {
                forgeSlotList[i].RefreshSlot(dataList[i]);
            }
            else    // ǥ���� ������ ����
            {
                forgeSlotList[i].ClearSlot();
            }
        }
    }

    public void OnClick_Enchant()   // qjxms snffuTdmfEo rkdghk tleh
    {
        if (TryEnchant())   // ��ȭ�õ� �� �ڿ� ����
        {
            selectItem.itemTableID++;
            GameManager.Inst.INVEN.UpdateItemInfo(selectItem);
            SelectItem(selectItem);
        }
        else    // ����
        {

        }
    }

    private bool TryEnchant()   //  ��ȭ �õ��� ó���ϴ� �Լ�
    {
        bool isSuccess = false;
        if (CanEnchant())
        {
            isSuccess = Random.Range(0, 10001) < 9000; // 90% ����Ȯ��
            GameManager.Inst.PlayerGold -= ((selectItem.itemTableID % 1000) * 500);
            RefreshData();
        }
        return false;
    }
    private bool CanEnchant()   //  ��ȭ �õ��� �ϱ� ���� ������ ���� �Ǵ��� Ȯ�� �ϴ� �Լ�.
    {
        if(selectItem.itemTableID % 1000 >= 5)  //  �ִ� ��ȭ �����ΰ�?
        {
            Debug.Log("�ƽ� ��ȭ ����");
            return false;
        }

        if (selectItem.itemTableID % 1000 * 500 > GameManager.Inst.PlayerGold)  //  ��ȭ ��� ����
        {
            Debug.Log("��ȭ �õ��� �ʿ��� ��尡 �����մϴ�.");
            return false;
        }

        return false;   //  ��ȭ�� �õ��� �� �� �ִ�.
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
