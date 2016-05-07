using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
		int count = Const.Const.charactors.Count;
		int randomCount = Random.Range (0, count);
		int i = 0;
		GameObject chara = null;
		string charaName = "";
		foreach(var key in Const.Const.charactors.Keys) {
			if (i == randomCount) {
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
		// TODO
		// 1. 6匹を超えていた場合の挙動
		// 2. ニックネームが登録ずみだった場合のアラート
		// 3. ステータスへのinsert
/*		Text  inputNickName = GameObject.Find("MonsterNickName").GetComponent<Text>();
		if (checkAlreadyNickName (inputNickName.text) == false) {
			Debug.Log("error: 登録済み -> " + inputNickName.text);
			return;
		}
		Text  hiddenMonsterName = GameObject.Find("HiddenMonsterName").GetComponent<Text>();
		Text  hiddenMonsterType = GameObject.Find("HiddenMonsterType").GetComponent<Text>();
		new Database.MyMonsterTable ().registMyMonster (inputNickName.text, hiddenMonsterName.text, int.Parse(hiddenMonsterType.text));
*/
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
}
