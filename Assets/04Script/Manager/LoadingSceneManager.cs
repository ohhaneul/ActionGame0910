using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //Image ������
using UnityEngine.SceneManagement;  //�� �ε�
using TMPro;    //�ؽ�Ʈ ����


public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField]
    private Image loadingBar;
    [SerializeField]
    private TextMeshProUGUI tipText;

    private void Awake()
    {
        loadingBar.fillAmount = 0;
        StartCoroutine("LoadAsyncScene");   //�񵿱� �ε� ó�� �ڷ�ƾ ����

     
    }
    IEnumerator LoadAsyncScene()
    {
        yield return null;
        tipText.text = GameManager.Inst.GetTipMessage(GameManager.Inst.NextScene);
        yield return YieldInstructionCache.WaitForSeconds(1f);  //����ũ �ε� 1��

        AsyncOperation async = SceneManager.LoadSceneAsync(GameManager.Inst.NextScene.ToString());
        async.allowSceneActivation = false; //�ε� �Ϸ� �� �ٷ� �� ������ �����ϱ� ���� (������ 100 ä��� ����

        float timeC = 0f;


        while (!async.isDone)
        {   //���� �����ָ� ���ѹݺ�/���� ����
            yield return null;
            timeC += Time.deltaTime;

            if (async.progress >= 0.9f)
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1f, timeC / 5f);
                if (loadingBar.fillAmount >= 0.99f)
                    async.allowSceneActivation = true;
            }
            else
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, async.progress, timeC);
                if (loadingBar.fillAmount >= async.progress)
                    timeC = 0f;
            }
        }

    }

}
