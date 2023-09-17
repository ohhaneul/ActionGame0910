using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class ActionGame : ScriptableObject
{
	public List<TableEntity_Item> ItemData; // Replace 'EntityType' to an actual type that is serializable.
	public List<TableEntity_Tip> TipMess; // Replace 'EntityType' to an actual type that is serializable.
	//public List<EntityType> Language; // Replace 'EntityType' to an actual type that is serializable.
	public List<TableEntity_Monster> MonsterData; // Replace 'EntityType' to an actual type that is serializable.
	public List<TableEntity_EXP> LevelEXP; // Replace 'EntityType' to an actual type that is serializable.
}
