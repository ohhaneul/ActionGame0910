using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //Image 접근을
using UnityEngine.SceneManagement;  //씬 로딩
using TMPro;    //텍스트 제공


public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField]
    private Image loadingBar;
    [SerializeField]
    private TextMeshProUGUI tipText;

    private void Awake()
    {
        loadingBar.fillAmount = 0;
        StartCoroutine("LoadAsyncScene");   //비동기 로딩 처리 코루틴 시작

     
    }
    IEnumerator LoadAsyncScene()
    {
        yield return null;
        tipText.text = GameManager.Inst.GetTipMessage(GameManager.Inst.NextScene);
        yield return YieldInstructionCache.WaitForSeconds(1f);  //페이크 로딩 1초

        AsyncOperation async = SceneManager.LoadSceneAsync(GameManager.Inst.NextScene.ToString());
        async.allowSceneActivation = false; //로딩 완료 후 바로 씬 변경을 방지하기 위해 (게이지 100 채우기 위해

        float timeC = 0f;


        while (!async.isDone)
        {   //리턴 안해주면 무한반복/씬이 멈춤
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
