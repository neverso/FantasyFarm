using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Const;

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
		Entity.MyMonster entity = mmt.selectMyMoster (PlayerPrefs.GetInt(Const.Const.nowMyMonsterType, 1));
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
		Debug.Log (prefabName);
		createdObj.GetComponent<Common>().isFarm = true;
	}
}
