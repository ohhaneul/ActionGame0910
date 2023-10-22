using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;    //로컬디바이스에 파일 생성/로딩 
public enum SceneName
{
    BaseScene,
    BossScene,
    BattleScene,
}

[SerializeField]
public class PlayerData //저장해야하는 데이터에 대한 구조
{
    public string userNickName;
    public int level;
    public int curExp;
    public int curHP;
    public int maxHP;
    public int curMP;
    public int maxMP;
    public int gold;
    public int uidCounter;  //고유한 itemID를 생성하기 위한 도구
    public Inventory inventory;

}

public class GameManager : Singleton<GameManager>
{
    private ActionGame table;

    private PlayerData pData = new PlayerData();    //객체 (데이터를 저장)
    public PlayerData PlayerInfo
    {
        get => pData;
    }


    private void Awake()
    {
        base.Awake();   //부모의 Awake 호출
        dataPath = Application.persistentDataPath + "/save";

        #region _TableData_
        table = Resources.Load<ActionGame>("ActionGame"); //런타임중에 에셋 폴더(Resources)에 접근해서 에셋을 동적으로 로딩


        // 아이템 테이블 딕셔너리로 검색
        for (int i = 0; i < table.ItemData.Count; i++)
        {
            //Debug.Log(table.ItemData[i].uid + " / " + table.ItemData.Count);
            itemTable.Add(table.ItemData[i].uid, table.ItemData[i]);
        }

        ////Debug.Log("팁 메세지 갯수 " + table.TipMess.Count);

        //팁 메세지 정리
        for (int i = 0; i < table.TipMess.Count; i++)
        {
            if (table.TipMess[i].sceneName == SceneName.BaseScene.ToString())
                baseTip.Add(table.TipMess[i]);
            else if (table.TipMess[i].sceneName == SceneName.BattleScene.ToString())
                battleTip.Add(table.TipMess[i]);
            else if (table.TipMess[i].sceneName == SceneName.BossScene.ToString())
                bossTip.Add(table.TipMess[i]);
        }

        ///* 메세지 오류확인 : 엑셀 파일 변수이름 마지막에 띄어쓰기 있었음
        //Debug.Log("베이스" + baseTip.Count);
        //Debug.Log("배틀" + battleTip.Count);
        //Debug.Log("보스" + bossTip.Count);
        //*/

        //List => Dictionary
        for (int i = 0; i < table.MonsterData.Count; i++)
        {
            monsterData.Add(table.MonsterData[i].uid, table.MonsterData[i]);
        }
        #endregion
        CheckData();
    }



    #region _TableData_

    private Dictionary<int, TableEntity_Item> itemTable = new Dictionary<int, TableEntity_Item>();
    //랩핑 함수 (보호되어야 하는 데이터를 읽기 전용으로 리턴
    public bool GetItemData(int itemID, out TableEntity_Item data)
    {
        return itemTable.TryGetValue(itemID, out data);
    }


    private List<TableEntity_Tip> battleTip = new List<TableEntity_Tip>();
    private List<TableEntity_Tip> baseTip = new List<TableEntity_Tip>();
    private List<TableEntity_Tip> bossTip = new List<TableEntity_Tip>();

    private int rand;

    //목표하는 씬과관련된 Tip 메세지를 랜덤하게 한 개 추출해온다
    public string GetTipMessage(SceneName scene)
    {
        string result = "";
        switch (scene)
        {
            case SceneName.BattleScene:
                result = battleTip[rand].tipText;
                break;
            case SceneName.BaseScene:
                result = baseTip[rand].tipText;
                break;
            case SceneName.BossScene:
                result = bossTip[rand].tipText;
                break;
        }
        return result;
    }

    private Dictionary<int, TableEntity_Monster> monsterData = new Dictionary<int, TableEntity_Monster>();
    //몬스터 ID 입력하면 몬스터 데이터 OUT
    public bool GetMonsterData(int itemID, out TableEntity_Monster monster)
    {
        return monsterData.TryGetValue(itemID, out monster);
    }




    public int GetNextEXPData(int curLevel)
    {
        if (table.LevelEXP.Count < curLevel)
            return -1;
        return table.LevelEXP[curLevel - 1].nextEXP;
    }

    #endregion

    //테이블 정보를 읽을 수 있는 랩핑함수
    //GetItemData
    //GetTipMessage
    //GetTipMessage
    //GetNextEXPData


    private SceneName nextScene;

    public SceneName NextScene
    {
        get => nextScene;
    }

    public void AsyncLoadNextScene(SceneName newScene)
    {
        // todo : 페이드 작업
        SaveData();
        nextScene = newScene;
        SceneManager.LoadScene("LoadingScene");
    }

    private void OnLevelWasLoaded(int level)    //  새로운 씬의 로딩이 완료되면 호출되는 이벤트
    {
        if(level > 2)
        Debug.Log("캐릭터 세이브 데이터 로딩.");
        LoadData();// 세이브 정보를 불러와서 갱신
    }

    #region _Save&Load_

    private string dataPath; // = Application.persistentDataPath + "/save"; //디바이스 정해진 폴더 save 폴더 추가.

    public void SaveData()
    {
        string data = JsonUtility.ToJson(pData);    //객체정보를 string 타입으로 변경 ( Json 구조를 활용)
        File.WriteAllText(dataPath, data);
    }

    public bool LoadData()
    {
        if (File.Exists(dataPath))
        {

            string data = File.ReadAllText(dataPath);
            pData = JsonUtility.FromJson<PlayerData>(data);
            return true;
        }
        return false;
    }

    public void DeleteData()
    {
        File.Delete(dataPath);
    }

    public bool CheckData()
    {
        if (File.Exists(dataPath))
        {
            return LoadData();
        }
        return false;
    }


    #endregion

    #region _UserData_
    public void CreateUserData(string newNickName)  //처음 계정 생성할 때 초기 데이터 세팅
    {
        pData.userNickName = newNickName;
        pData.curExp = 0;
        pData.gold = 5000;
        pData.level = 1;
        pData.curHP = pData.maxHP = 50;
        pData.curMP = pData.maxMP = 30;
        pData.uidCounter = 0;
        pData.inventory = new Inventory();
    }

    public int PlayerUidMaker
    {
        get
        {
            return ++pData.uidCounter; // 정대 중복 값을 만들지 않기 위함
        }
    }

    public string PlayerName
    {
        get => pData.userNickName;
    }

    public int PlayerLevel
    {
        get => pData.level;
    }


    public int PlayerCurrentExp
    {
        get => pData.curExp;
    }

    public void AddEXP(int addEXP)
    {
        pData.curExp += addEXP;
        //todo 레벨업 처리
    }

    public int PlayerGold
    {
        get => pData.gold;
        set => pData.gold = value;
    }

    public void AddGold(int addGold)
    {
        pData.gold += addGold;
        //todo : ui 갱신처리
    }

    public void ChangeHP(int hp) // +-
    {
        pData.curHP += hp;
        if (pData.curHP > pData.maxHP)
            pData.curHP = pData.maxHP;
        else if (pData.curHP < 0)
            pData.curHP = 0;
        UpdateHPMP();
    }

    public void ChangeMP(int mp) // +-
    {
        pData.curHP += mp;
        if (pData.curMP > pData.maxMP)
            pData.curMP = pData.maxMP;
        else if (pData.curHP < 0)
            pData.curMP = 0;

        UpdateHPMP();
    }

    private void UpdateHPMP()
    {
        Debug.Log("왼쪽 상단 프로필 갱신");
    }

    #endregion

    #region _inventory_
    public bool LootingItem(InventoryItemData newItem)
    {
        if (!pData.inventory.isFull())
        {
            pData.inventory.AddItem(newItem);

            Debug.Log("아이템 습득" + newItem.itemTableID + "인벤토리 내 아이템 갯수는" + pData.inventory.CurSlot);
            return true;
        }

        return false;
    }

    public Inventory INVEN  {   get => pData.inventory;  }
    #endregion

}
