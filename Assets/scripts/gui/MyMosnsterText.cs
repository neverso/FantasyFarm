using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MyMosnsterText : MonoBehaviour {
	// モンスターリスト
	public GameObject myMonsterList;
	// 自分自身(モンスターリストボタン)
	public Text dispMyMonsterButtonText;
	private bool isDisp = false;

	public void start() {
	}
	public void update() {
	}

	public void showMyMonsters() {
		Database.MyMonsterTable table = new Database.MyMonsterTable ();
		List<Entity.MyMonster> list = table.selectMyMosters ();
		if (isDisp) {
			myMonsterList.SetActive (false);
			isDisp = false;
			dispMyMonsterButtonText.text = "モンスター一覧";
		} else {
			myMonsterList.SetActive (true);
			int index = 1;
			foreach (Entity.MyMonster monster in list) {
				Button changeMonsterButton = GameObject.Find("MyMonster_" + index).GetComponent<Button>();
				// ButtonのテキストにDBから取得した名前をあてはめる
				Text text = changeMonsterButton.GetComponentInChildren<Text>();
				text.text = monster.getDispname();
				index++;
			}
			isDisp = true;
			dispMyMonsterButtonText.text = "閉じる";
		}
	}

	/**
	 * MyMonster一覧から選択したモンスターに切り替える
	 */
	public void changemyMonster(){
		// クリックしたボタンのテキストを取得
		Text text = gameObject.GetComponentInChildren<Text>();
		// Monster一覧配列にない文字列なら無視
		string identificationName = getIdentificationName (text.text);
		if (!Const.Const.monsterTypes.ContainsKey (identificationName)) {
			return;
		}
		Debug.Log(Const.Const.monsterTypes[identificationName]);
		// ユーザーがクリックしたモンスターのtypeをPlayersPrefに保存し、FarmSceneを再度ロードする。
		PlayerPrefs.SetInt (Const.Const.nowMyMonsterType, Const.Const.monsterTypes[identificationName]);
		new SceneManager ().startGame ();
	}

	/*
	 *Myモンスターの表示名から識別名を取得する
	 */
	string getIdentificationName (string dispName)
	{
		Entity.MyMonster entity = new Database.MyMonsterTable ().selectMyMonsterIdentificationName (dispName);
		return entity.getName ();
	}
}
