using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Const;

public class TrainingScene : MonoBehaviour {
	// トレーニング終了フラグ
	private bool isGameStop = false;
	// カウントダウンテキスト
	public Text _textCountdown;
	// 完了ボタン
	public Button completeButton;
	// TimeUpText
	public Text timeUpText;
	private float time = 120f;
	private bool isStart = false;
	// キャラクター
	GameObject createdObj;

	// Use this for initialization
	void Start () {
		init ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isGameStop == false) {
			countDownTimeUp ();
		}
	}

	void init ()
	{
		// 完了ボタンを無効の無色透明にする
 		completeButton.interactable = false;
		completeButton.image.color = Colors.Alpha(Colors.White, 0f);
		completeButton.GetComponentInChildren<Text>().color = Colors.Alpha(Colors.Black, 0f);

		// 制限時間の初期化
		timeUpText.text = ((int)time).ToString();
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
			createdObj = (GameObject)Instantiate (prefab, new Vector3 (0, -5, 0), Quaternion.Euler (0, 180, 0));
		} else {
			createdObj = (GameObject)Instantiate (prefab, new Vector3 (0, -5, 0), Quaternion.Euler (0, 0, 0));
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
		AttackButton.isReadyState = true;
		// 制限時間カウントダウンの開始フラグ
		isStart = true;
	}

	/**
	 * 制限時間のカウントダウン
	 */
	void countDownTimeUp ()
	{
		if (isStart) {
			//1秒に1ずつ減らしていく
			time -= Time.deltaTime;
			//マイナスは表示しない
			if (time < 0) {
				time = 0;
			}
			timeUpText.text = ((int)time).ToString ();
			if (time == 0) {
				completeButton.interactable = true;
				completeButton.image.color = Colors.Alpha(Colors.White, 1f);
				completeButton.GetComponentInChildren<Text>().color = Colors.Alpha(Colors.Black, 1f);
				isGameStop = true;
				AttackButton.isReadyState = false;
				Destroy(createdObj);
				_textCountdown.gameObject.SetActive(true);
				_textCountdown.text = "TIME UP!!";
			}
		}
	}
}
