using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseTownPopup
{
    public void PopupOpen();
    public void PopupClose();
}

public class Popup_Base : MonoBehaviour, IBaseTownPopup
{
    public void PopupClose()
    {
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }

    public void PopupOpen()
    {
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }

    private void Awake()
    {
        gameObject.LeanScale(Vector3.zero, 0.01f);  //  게임 시작하면 기본적으로 팝업은 닫힌상태로 시작
    }

}
