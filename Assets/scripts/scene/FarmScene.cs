using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Const;
using System.Collections.Generic;

public class FarmScene : MonoBehaviour {
	// モンスター表示名
	public Text nickNameText;
	// モンスター年齢
	public Text ageText;
	// モンスターリスト
	public GameObject myMonsterList;
	// ステータス
	public GameObject status;

	// Use this for initialization
	void Start () {
		// TODO 試験データ
//		PlayerPrefs.SetInt (Const.Const.nowMyMonsterID, 1);
//		new Database.MyMonsterTable().deleteAll();
//		new Database.MyMonsterTable().registMyMonster("だるまん", "daruman", 1);
//		new Database.MyMonsterTable().registMyMonster("あり", "antrope", 2);
//		new Database.MyMonsterTable().registMyMonster("ドラえもん", "aodanuki", 3);
//		new Database.MyMonsterTable().registMyMonster("ナメクジ", "crayslug", 4);
//		new Database.MyMonsterTable().registMyMonster("ゴーレム", "goalem", 5);
//		new Database.MyMonsterTable().registMyMonster("ゴースト", "goast", 7);

		init ();
		dispFarmMonster (getMyMonstername());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void init ()
	{
		myMonsterList.SetActive (false);
		status.SetActive (false);
	}

	Entity.MyMonster getMyMonstername ()
	{
		Database.MyMonsterTable mmt = new Database.MyMonsterTable ();
		Entity.MyMonster entity = mmt.selectMyMoster (PlayerPrefs.GetInt(Const.Const.nowMyMonsterID, 1));
		return entity;
	}

	void dispFarmMonster (Entity.MyMonster myMonster)
	{
		string prefabName = Const.Const.charactors[myMonster.getName()];
		nickNameText.text = myMonster.getDispname();
		ageText.text = "" + myMonster.getOld () + "才";
		GameObject prefab = (GameObject)Resources.Load (prefabName);
		// プレハブからインスタンスを生成
		GameObject createdObj = (GameObject)Instantiate (prefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 180, 0));
		createdObj.GetComponent<Common>().isFarm = true;
	}

	private int selectFirstMyMonsterId()
	{
		int id = 0;
		List<Entity.MyMonster> entities = new Database.MyMonsterTable ().selectMyMosters ();
		if (entities != null) {
			id = entities[0].getType ();
		}
		return id;
	}
}
