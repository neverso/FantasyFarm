using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Const;

public class TrainingScene : MonoBehaviour {
	// カウントダウンテキスト
	public Text _textCountdown;
	// 完了ボタン
	public Button completeButton;
	// カメラ
	public Camera camera;

	// Use this for initialization
	void Start () {
		init ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void init ()
	{
		// 完了ボタンを無効の無色透明にする
/*
 		completeButton.interactable = false;
		completeButton.image.color = Colors.Alpha(Colors.White, 0f);
		completeButton.GetComponentInChildren<Text>().color = Colors.Alpha(Colors.Black, 0f);
*/
		StartCoroutine(countdownCoroutine());
	}

	Entity.MyMonster getMyMonstername ()
	{
		Database.MyMonsterTable mmt = new Database.MyMonsterTable ();
		Entity.MyMonster entity = mmt.selectMyMoster (PlayerPrefs.GetInt(Const.Const.nowMyMonsterID, 1));
		return entity;
	}

	void dispMyMonster (Entity.MyMonster myMonster)
	{
		string prefabName = Const.Const.charactors[myMonster.getName()];
		GameObject prefab = (GameObject)Resources.Load (prefabName);
		// プレハブからインスタンスを生成
		if (myMonster.getName ().Equals ("crayslug")) {
			GameObject createdObj = (GameObject)Instantiate (prefab, new Vector3 (0, 0, 0), Quaternion.Euler (0, 180, 0));
		} else {
			GameObject createdObj = (GameObject)Instantiate (prefab, new Vector3 (0, 0, 0), Quaternion.Euler (0, 0, 0));
		}
	}

	IEnumerator countdownCoroutine()
	{
		_textCountdown.gameObject.SetActive(true);
		
		_textCountdown.text = "3";
		yield return new WaitForSeconds(1.0f);
		
		_textCountdown.text = "2";
		yield return new WaitForSeconds(1.0f);
		
		_textCountdown.text = "1";
		yield return new WaitForSeconds(1.0f);
		
		_textCountdown.text = "GO!";
		yield return new WaitForSeconds(1.0f);
		
		_textCountdown.text = "";
		_textCountdown.gameObject.SetActive(false);
		dispMyMonster (getMyMonstername());
		CameraControl.isReady = true;
	}
}
