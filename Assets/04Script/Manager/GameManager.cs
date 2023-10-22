using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;    //���õ���̽��� ���� ����/�ε� 
public enum SceneName
{
    BaseScene,
    BossScene,
    BattleScene,
}

[SerializeField]
public class PlayerData //�����ؾ��ϴ� �����Ϳ� ���� ����
{
    public string userNickName;
    public int level;
    public int curExp;
    public int curHP;
    public int maxHP;
    public int curMP;
    public int maxMP;
    public int gold;
    public int uidCounter;  //������ itemID�� �����ϱ� ���� ����
    public Inventory inventory;

}

public class GameManager : Singleton<GameManager>
{
    private ActionGame table;

    private PlayerData pData = new PlayerData();    //��ü (�����͸� ����)
    public PlayerData PlayerInfo
    {
        get => pData;
    }


    private void Awake()
    {
        base.Awake();   //�θ��� Awake ȣ��
        dataPath = Application.persistentDataPath + "/save";

        #region _TableData_
        table = Resources.Load<ActionGame>("ActionGame"); //��Ÿ���߿� ���� ����(Resources)�� �����ؼ� ������ �������� �ε�


        // ������ ���̺� ��ųʸ��� �˻�
        for (int i = 0; i < table.ItemData.Count; i++)
        {
            //Debug.Log(table.ItemData[i].uid + " / " + table.ItemData.Count);
            itemTable.Add(table.ItemData[i].uid, table.ItemData[i]);
        }

        ////Debug.Log("�� �޼��� ���� " + table.TipMess.Count);

        //�� �޼��� ����
        for (int i = 0; i < table.TipMess.Count; i++)
        {
            if (table.TipMess[i].sceneName == SceneName.BaseScene.ToString())
                baseTip.Add(table.TipMess[i]);
            else if (table.TipMess[i].sceneName == SceneName.BattleScene.ToString())
                battleTip.Add(table.TipMess[i]);
            else if (table.TipMess[i].sceneName == SceneName.BossScene.ToString())
                bossTip.Add(table.TipMess[i]);
        }

        ///* �޼��� ����Ȯ�� : ���� ���� �����̸� �������� ���� �־���
        //Debug.Log("���̽�" + baseTip.Count);
        //Debug.Log("��Ʋ" + battleTip.Count);
        //Debug.Log("����" + bossTip.Count);
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
    //���� �Լ� (��ȣ�Ǿ�� �ϴ� �����͸� �б� �������� ����
    public bool GetItemData(int itemID, out TableEntity_Item data)
    {
        return itemTable.TryGetValue(itemID, out data);
    }


    private List<TableEntity_Tip> battleTip = new List<TableEntity_Tip>();
    private List<TableEntity_Tip> baseTip = new List<TableEntity_Tip>();
    private List<TableEntity_Tip> bossTip = new List<TableEntity_Tip>();

    private int rand;

    //��ǥ�ϴ� �������õ� Tip �޼����� �����ϰ� �� �� �����ؿ´�
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
    //���� ID �Է��ϸ� ���� ������ OUT
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

    //���̺� ������ ���� �� �ִ� �����Լ�
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
        // todo : ���̵� �۾�
        SaveData();
        nextScene = newScene;
        SceneManager.LoadScene("LoadingScene");
    }

    private void OnLevelWasLoaded(int level)    //  ���ο� ���� �ε��� �Ϸ�Ǹ� ȣ��Ǵ� �̺�Ʈ
    {
        if(level > 2)
        Debug.Log("ĳ���� ���̺� ������ �ε�.");
        LoadData();// ���̺� ������ �ҷ��ͼ� ����
    }

    #region _Save&Load_

    private string dataPath; // = Application.persistentDataPath + "/save"; //����̽� ������ ���� save ���� �߰�.

    public void SaveData()
    {
        string data = JsonUtility.ToJson(pData);    //��ü������ string Ÿ������ ���� ( Json ������ Ȱ��)
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
    public void CreateUserData(string newNickName)  //ó�� ���� ������ �� �ʱ� ������ ����
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
            return ++pData.uidCounter; // ���� �ߺ� ���� ������ �ʱ� ����
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
        //todo ������ ó��
    }

    public int PlayerGold
    {
        get => pData.gold;
        set => pData.gold = value;
    }

    public void AddGold(int addGold)
    {
        pData.gold += addGold;
        //todo : ui ����ó��
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
        Debug.Log("���� ��� ������ ����");
    }

    #endregion

    #region _inventory_
    public bool LootingItem(InventoryItemData newItem)
    {
        if (!pData.inventory.isFull())
        {
            pData.inventory.AddItem(newItem);

            Debug.Log("������ ����" + newItem.itemTableID + "�κ��丮 �� ������ ������" + pData.inventory.CurSlot);
            return true;
        }

        return false;
    }

    public Inventory INVEN  {   get => pData.inventory;  }
    #endregion

}
