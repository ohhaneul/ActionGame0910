using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InventorySlot : MonoBehaviour
{
    private bool isEmpty;   //  슬롯이 비워져 있는 상태인지
    
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

    private bool isSelect;  //  슬롯 선택여부

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
    //  슬롯에 보여지는 정부(아이콘, 보유개수)를 갱신
    public void DrawItem(InventoryItemData newItem)
    {
        if (GameManager.Inst.GetItemData(newItem.itemTableID, out TableEntity_Item itemData))
        {
            icon.sprite = Resources.Load<Sprite>(itemData.iconImg); //  에셋에 있는 스프라이트 이미지를 불러옴
            ChangeAmount(newItem.amount);
            isEmpty = false;
            icon.enabled = true;
        }
        else
            Debug.Log("InventorySlot.cs - DrawItem() - itemData 받아오기 실패"+ newItem.itemTableID);
    }
    //슬롯을 비우는 함수
    public void ClearSlot()
    {
        focus.SetActive(false);
        isSelect = false;
        amount.enabled = false;
        isEmpty = true;
        icon.enabled = false;
    }
    //아이템의 보유개수를 변경
    public void ChangeAmount(int newAmount)
    {
        if (newAmount < 2)  //  아이템이 1개인 경우
        {
            amount.enabled = false;
        }
        else    //  2개 이상인 경우
        {
            amount.enabled = true;
            amount.text = newAmount.ToString();
        }

    }

    //  선택여부를 변경하는 함수
    public void SelectButton()
    {
        if (!isEmpty)
        {
            isSelect = !isSelect;
            SelectSlot(isSelect);
        }
    }


    // 선택여부를 화면에 표기.
    public void SelectSlot(bool isSelect)
    {
        focus.SetActive(isSelect);
    }
}
