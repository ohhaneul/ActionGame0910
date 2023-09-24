using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  C# 클래스 다중상속 허용하지 않음.( 인터페이스는 다중상속이 가능하다.)
public class Popup_Shop : Popup_Base, IBaseTownPopup
{
    public void PopupClose()
    {
        gameObject.LeanScale(Vector3.zero, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }

    public void PopupOpen()
    {
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }
}
