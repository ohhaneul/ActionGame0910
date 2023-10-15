using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ForgeSlot : MonoBehaviour
{
    private Popup_Forge forgePopup; // �ش� ������ ���� �θ� ������Ʈ ��ũ��Ʈ
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Image focus;
    private bool isFocus;

    public bool ISFocus
    {
        set
        {
            ISFocus = value;
            focus.enabled = value;
        }
    }

    private InventoryItemData data;
    private Button selectBtn;

    public void CreateSlot(Popup_Forge forge)   // �˾�â���� �ش� ���� ������ ȣ��
    {
        if (TryGetComponent<Button>(out selectBtn))
        {
            Debug.Log("ForgeSlot.cs - CreateSlot() - selectBton ���� ����");
        }
        else
        {
            selectBtn.onClick.AddListener(OnClick_SelectBtn);   //��ư�� �̺�Ʈ ������ ���, <�̰� �� ���� ���>
        }

        forgePopup = forge;
        gameObject.SetActive(false);
    }


    // �ش� ���� ���� �Ǿ��� ��
    private void OnClick_SelectBtn()
    {
        if (!isFocus)
        {
            SelectFocus(true);
            forgePopup.SelectItem(data);
        }
    }

    public void SelectFocus(bool select)
    {
        isFocus = select;
    }

    public void RefreshSlot(InventoryItemData item)
    {
        gameObject.SetActive(true);
        data = item;
        if (GameManager.Inst.GetItemData(item.itemTableID, out TableEntity_Item itemData))  // ���̺� ���� �޾ƿ���
        {
            icon.sprite = Resources.Load<Sprite>(itemData.iconImg);
            ISFocus = false;
        }
    }

    public void ClearSlot()
    {
        gameObject.SetActive(false);
    }
}
