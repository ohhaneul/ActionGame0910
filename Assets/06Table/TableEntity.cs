[System.Serializable]   //Á÷·ÄÈ­
public class TableEntity_Tip
{
    public int uid;
    public string sceneName;
    public string tipText;
}

[System.Serializable]
public class TableEntity_Item
{
    public int uid;
    public string name;
    public string iconImg;
    public int sellGold;
    public int attackDamage;
    public int attackRange;
    public int attackRate;
    public bool equip;
}

[System.Serializable]
public class TableEntity_Monster
{
    public int uid;
    public string monsterName;
    public int monsterType;
    public int moveSpeed;
    public int attackDamage;
    public int maxHP;
    public int dropID;
    public int dropRate;
    public int EXP;
}

[System.Serializable]
public class TableEntity_EXP
{
    public int curLevel;
    public int nextEXP;
}