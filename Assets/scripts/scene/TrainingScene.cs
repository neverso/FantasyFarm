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

	// Use this for initialization
	void Start () {
		QualitySettings.vSyncCount = 0; // VSyncをOFFにする
		Application.targetFrameRate = 120; // ターゲットフレームレートを60に設定
		init ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isGameStop == false) {
			countDownTimeUp ();
			addEnemies();
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
		// AIに生成
		createAIEnemies();
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
			Instantiate (prefab, new Vector3 (0, -5, 0), Quaternion.Euler (0, 180, 0));
		} else {
			Instantiate (prefab, new Vector3 (0, -5, 0), Quaternion.Euler (0, 0, 0));
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
				CameraControl.isReady = false;
				AttackButton.isReadyState = false;
				_textCountdown.gameObject.SetActive(true);
				_textCountdown.text = "TIME UP!!";
			}
		}
	}

	/**
	 * 敵AIの生成
	 */
	void createAIEnemies ()
	{
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
		// 10匹生成
		for (i = 0; i < 10; i++) {
			GameObject enemy = (GameObject)Instantiate (chara, new Vector3(Random.Range (10, 400), -4, Random.Range (10, 400)), Quaternion.Euler(0, 180, 0));
			enemy.tag = "enemy";
			Common common = enemy.GetComponent<Common>();
			common.isFarm = true;
			common.speed = 10;
			common.rotationSmooth = 20;
			common.levelSize = 100;

			i++;
		}
	}

	void addEnemies ()
	{
		GameObject[] oldPrefab = GameObject.FindGameObjectsWithTag("enemy");
		if (oldPrefab.Length < 5) {
			createAIEnemies();
		}
	}
}
