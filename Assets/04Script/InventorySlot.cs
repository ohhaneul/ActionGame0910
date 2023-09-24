using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InventorySlot : MonoBehaviour
{
    private bool isEmpty;   //  ������ ����� �ִ� ��������
    
    public bool EMPTY
    {
        get => isEmpty;
    }

    private int slotIndex;
    public int SLOTINDEX
    {
        get => slotIndex;
        set => slotIndex = value;
    }

    private Image icon;
    private GameObject focus;
    private TextMeshProUGUI amount;
    private Button button;

    private bool isSelect;  //  ���� ���ÿ���

    private void Awake()
    {
        icon = transform.GetChild(0).GetComponent<Image>();
        focus = transform.GetChild(1).gameObject;
        amount = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        button = GetComponent<Button>();

        button.onClick.AddListener(SelectButton);

        ClearSlot();
    }

    private Color icoNColor;
    //  ���Կ� �������� ����(������, ��������)�� ����
    public void DrawItem(InventoryItemData newItem)
    {
        if (GameManager.Inst.GetItemData(newItem.itemTableID, out TableEntity_Item itemData))
        {
            icon.sprite = Resources.Load<Sprite>(itemData.iconImg); //  ���¿� �ִ� ��������Ʈ �̹����� �ҷ���
            ChangeAmount(newItem.amount);
            isEmpty = false;
            icon.enabled = true;
        }
        else
            Debug.Log("InventorySlot.cs - DrawItem() - itemData �޾ƿ��� ����"+ newItem.itemTableID);
    }
    //������ ���� �Լ�
    public void ClearSlot()
    {
        focus.SetActive(false);
        isSelect = false;
        amount.enabled = false;
        isEmpty = true;
        icon.enabled = false;
    }
    //�������� ���������� ����
    public void ChangeAmount(int newAmount)
    {
        if (newAmount < 2)  //  �������� 1���� ���
        {
            amount.enabled = false;
        }
        else    //  2�� �̻��� ���
        {
            amount.enabled = true;
            amount.text = newAmount.ToString();
        }

    }

    //  ���ÿ��θ� �����ϴ� �Լ�
    public void SelectButton()
    {
        if (!isEmpty)
        {
            isSelect = !isSelect;
            SelectSlot(isSelect);
        }
    }


    // ���ÿ��θ� ȭ�鿡 ǥ��.
    public void SelectSlot(bool isSelect)
    {
        focus.SetActive(isSelect);
    }
}
