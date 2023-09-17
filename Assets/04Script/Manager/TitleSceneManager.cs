using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI welcomText;

    [SerializeField]
    private GameObject nickNamePopup;   //계정 생성용 팝업

    [SerializeField]
    private TextMeshProUGUI warningText;

    private bool havePlayerInfo;    //계정이 생성되어 있는 상태인지

    private void Awake()
    {
        InitTitleScene();
    }

    private void InitTitleScene()
    {
        if (GameManager.Inst.CheckData())   //데이터 파일 확인하고, 로딩
        {
            havePlayerInfo = true;
            welcomText.text = GameManager.Inst.PlayerName + "님 환영합니다. \n 터치시 시작합니다.";
        }
        else
        {
            welcomText.text = "계정을 생성하려면 터치하세요.";
            havePlayerInfo = false;
        }
    }

    //  UI 버튼 이벤트

    public void EnterBtn()
    {
        if (havePlayerInfo) //  접속
        {
            GameManager.Inst.AsyncLoadNextScene(SceneName.BaseScene);
        }
        else                //  계정 생성로직
        {
            LeanTween.scale(nickNamePopup, Vector3.one, 0.7f).setEase(LeanTweenType.easeOutElastic);
            welcomText.enabled = false;
        }

    }

    public void DeleteBtn()
    {
        GameManager.Inst.DeleteData();
        InitTitleScene();   //타이틀씬 초기화
    }

    private string newNickName;

    public  void InputField_Nick(string input)
    {
        newNickName = input;
    }

    public void CreateUserInfo()
    {
        if (newNickName.Length >=2) //  두 글자 이상(, 금지어, 중복체크)
        {
            LeanTween.scale(nickNamePopup, Vector3.zero, 0.7f).setEase(LeanTweenType.easeOutElastic);   //  팝업창 제거
            welcomText.enabled = true;
            GameManager.Inst.CreateUserData(newNickName);
            GameManager.Inst.SaveData();
            InitTitleScene();
        }
        else
        {
            WarningTextActive();    //  다시 입력하라는 경고 메세지 출력
        }
    }

    Color fromColor = Color.red;
    Color toColor = Color.red;


    private void WarningTextActive()
    {
        fromColor.a = 0f;
        toColor.a = 1f;

        LeanTween.value(warningText.gameObject, UpdateValue, fromColor, toColor, 1f).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.value(warningText.gameObject, UpdateValue, toColor, fromColor, 1f).setDelay(1.5f).setEase(LeanTweenType.easeInOutQuad);
    }

    private void UpdateValue(Color val)
    {
        warningText.color = val;
    }
}
