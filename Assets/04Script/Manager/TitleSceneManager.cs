using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI welcomText;

    [SerializeField]
    private GameObject nickNamePopup;   //���� ������ �˾�

    [SerializeField]
    private TextMeshProUGUI warningText;

    private bool havePlayerInfo;    //������ �����Ǿ� �ִ� ��������

    private void Awake()
    {
        InitTitleScene();
    }

    private void InitTitleScene()
    {
        if (GameManager.Inst.CheckData())   //������ ���� Ȯ���ϰ�, �ε�
        {
            havePlayerInfo = true;
            welcomText.text = GameManager.Inst.PlayerName + "�� ȯ���մϴ�. \n ��ġ�� �����մϴ�.";
        }
        else
        {
            welcomText.text = "������ �����Ϸ��� ��ġ�ϼ���.";
            havePlayerInfo = false;
        }
    }

    //  UI ��ư �̺�Ʈ

    public void EnterBtn()
    {
        if (havePlayerInfo) //  ����
        {
            GameManager.Inst.AsyncLoadNextScene(SceneName.BaseScene);
        }
        else                //  ���� ��������
        {
            LeanTween.scale(nickNamePopup, Vector3.one, 0.7f).setEase(LeanTweenType.easeOutElastic);
            welcomText.enabled = false;
        }

    }

    public void DeleteBtn()
    {
        GameManager.Inst.DeleteData();
        InitTitleScene();   //Ÿ��Ʋ�� �ʱ�ȭ
    }

    private string newNickName;

    public  void InputField_Nick(string input)
    {
        newNickName = input;
    }

    public void CreateUserInfo()
    {
        if (newNickName.Length >=2) //  �� ���� �̻�(, ������, �ߺ�üũ)
        {
            LeanTween.scale(nickNamePopup, Vector3.zero, 0.7f).setEase(LeanTweenType.easeOutElastic);   //  �˾�â ����
            welcomText.enabled = true;
            GameManager.Inst.CreateUserData(newNickName);
            GameManager.Inst.SaveData();
            InitTitleScene();
        }
        else
        {
            WarningTextActive();    //  �ٽ� �Է��϶�� ��� �޼��� ���
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
