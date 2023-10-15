using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ForgeSlot : MonoBehaviour
{
    private Popup_Forge forgePopup; // 해당 슬롯을 만들어낸 부모 오브젝트 스크립트
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

    public void CreateSlot(Popup_Forge forge)   // 팝업창에서 해당 슬롯 만들어낼때 호출
    {
        if (TryGetComponent<Button>(out selectBtn))
        {
            Debug.Log("ForgeSlot.cs - CreateSlot() - selectBton 참조 실패");
        }
        else
        {
            selectBtn.onClick.AddListener(OnClick_SelectBtn);   //버튼의 이벤트 리스너 등록, <이게 더 좋은 방법>
        }

        forgePopup = forge;
        gameObject.SetActive(false);
    }


    // 해당 슬롯 선택 되었을 때
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
        if (GameManager.Inst.GetItemData(item.itemTableID, out TableEntity_Item itemData))  // 테이블 정보 받아오기
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
