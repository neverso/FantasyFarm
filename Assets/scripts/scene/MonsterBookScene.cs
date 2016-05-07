using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Const;
using System.Collections.Generic;

/**
 * モンスター図鑑シーン
 */ 
public class MonsterBookScene : MonoBehaviour {
	// 説明文
	public Text description;

	// ファーストビューではだるまんを表示させる
	void Start () {
		// モンスター
		GameObject prefab = (GameObject)Resources.Load ("prefabs/daruman");
		GameObject createdObj = (GameObject)Instantiate (prefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 180, 0));
		createdObj.GetComponent<Common> ().isBook = true;
		// 説明文
		description.text = getDescription ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private string getDescription ()
	{
		Database.MonstersTable mmt = new Database.MonstersTable ();
		Entity.Monsters entity = mmt.selectMosterInfo ("daruman");
		return entity.getDescription ();
	}
}
