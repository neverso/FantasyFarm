using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BirthScene : MonoBehaviour {

	public GameObject action1;
	public GameObject registUI;

	// Use this for initialization
	void Start () {
		// コルーチン呼び出し
		StartCoroutine("birthMonster");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator  birthMonster() {
		if (registUI != null) {
			registUI.SetActive(false);
		}

		yield return new WaitForSeconds(10);
		// 光の削除
		Destroy (action1);
		// 爆発
		GameObject prefab = (GameObject)Resources.Load ("prefabs/BirthAction");
		Instantiate (prefab);
		// キャラクター誕生
		// TODO 試験
		int birthMonsterIndex = 6; //PlayerPrefs.GetInt (Const.Const.birthMonsterID);
		int i = 0;
		GameObject chara = null;
		string charaName = "";
		foreach(var key in Const.Const.charactors.Keys) {
			if (i == birthMonsterIndex) {
				chara = (GameObject)Resources.Load (Const.Const.charactors[key]);
				charaName = key;
				break;
			}
			i++;
		}
		GameObject oldPrefab = GameObject.FindGameObjectWithTag("monster");
		if (oldPrefab != null) {
			GameObject.Destroy(oldPrefab);
		}
		if (charaName.Equals ("crayslug")) {
			Instantiate (chara, new Vector3 (-10, 10, -10), Quaternion.Euler (0, 0, 0));
		} else {
			Instantiate (chara, new Vector3(-10, 10, -10), Quaternion.Euler(0, 180, 0));
		}
		// 登録用にtypeを保存
		Text  hiddenMonserName = GameObject.Find("HiddenMonsterName").GetComponent<Text>();
		hiddenMonserName.text = charaName;
		Text  hiddenMonserType = GameObject.Find("HiddenMonsterType").GetComponent<Text>();
		hiddenMonserType.text = "" + Const.Const.monsterTypes [charaName];
		registUI.SetActive(true);
	}

	// MyMonsterに登録
	public void registAndMoveFarm() {
		Text  inputNickName = GameObject.Find("MonsterNickName").GetComponent<Text>();
		Text  hiddenMonsterName = GameObject.Find("HiddenMonsterName").GetComponent<Text>();
		Text  hiddenMonsterType = GameObject.Find("HiddenMonsterType").GetComponent<Text>();
		if (checkAlreadyNickName (inputNickName.text) == false) {
			string errorMsg = "この名前は登録済みです。";
			if (checkHasMonsterType (hiddenMonsterType.text) == false) {
				errorMsg = "このモンスターはすでに所有しています。";
			}
			if (checkMyMonsterCount() == false) {
				errorMsg = "モンスターがいっぱいです。";
			}
			Debug.Log(errorMsg);
			AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
			activity.Call ("runOnUiThread", new AndroidJavaRunnable (() => {
				AndroidJavaObject alertDialogBuilder = new AndroidJavaObject ("android.app.AlertDialog$Builder", activity);
				alertDialogBuilder.Call<AndroidJavaObject> ("setMessage", errorMsg);
				alertDialogBuilder.Call<AndroidJavaObject> ("setCancelable", true);
				alertDialogBuilder.Call<AndroidJavaObject> ("setPositiveButton", "OK", null);
				AndroidJavaObject dialog = alertDialogBuilder.Call<AndroidJavaObject> ("create");
				dialog.Call ("show");
			}));
			return;
		}
		// MyMonsterに登録
		new Database.MyMonsterTable ().registMyMonster (inputNickName.text, hiddenMonsterName.text, int.Parse(hiddenMonsterType.text));
		// MonsterにMyMonsterフラグを登録
		new Database.MonstersTable ().updateMyMonsterFlg (int.Parse(hiddenMonsterType.text), 1);

		// Scene移動
		new SceneManager ().startGame ();
	}

	/**
	 * 入力したニックネームが既に登録ずみかどうかをチェックする
	 */
	private bool checkAlreadyNickName (string dispName)
	{
		bool ret = false;
		Entity.MyMonster entity = new Database.MyMonsterTable ().selectMyMosterByNickName (dispName);
		if (entity == null) {
			ret = true;
		}
		return ret;
	}

	/**
	 * MyMonsterにすでに登録済みのtypeかどうかをチェックする
	 */
	private bool checkHasMonsterType(string type)
	{
		bool ret = false;
		Entity.MyMonster entity = new Database.MyMonsterTable ().selectMyMosterByType (type);
		if (entity == null) {
			ret = true;
		}
		return ret;
	}

	private bool checkMyMonsterCount()
	{
		bool ret = false;
		List<Entity.MyMonster> entities = new Database.MyMonsterTable ().selectMyMosters();
		if (entities.Count < 6) {
			ret = true;
		}
		return ret;
	}
}
