using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StatusText : MonoBehaviour {
	// Statusリスト
	public GameObject status;
	// 自分自身(Statusボタン)
	public Text dispStatusButtonText;
	public Slider life;
	public Text lifeText;
	public Slider power;
	public Text powerText;
	public Slider wise;
	public Text wiseText;
	public Slider hit;
	public Text hitText;
	public Slider avoid;
	public Text avoidText;
	public Slider deffence;
	public Text deffenceText;

	private bool isDisp = false;
	
	public void start() {
	}
	public void update() {
	}
	
	public void showStatus() {
		// PlayersPrefからtypeを取得し、typeに応じたステータスを取得する
		Database.StatusTable table = new Database.StatusTable ();
		Entity.Status result = table.selectStatusByType (PlayerPrefs.GetInt(Const.Const.nowMyMonsterType, 1));
		Debug.Log (result.getType() + result.getLife() + result.getPower() + result.getWise() + result.getHit() + result.getAvoid() + result.getDeffence());
		if (isDisp) {
			status.SetActive (false);
			isDisp = false;
			dispStatusButtonText.text = "ステータス";
		} else {
			status.SetActive (true);
			life.value = result.getLife();
			lifeText.text = "ライフ(" + result.getLife() + ")";
			power.value = result.getPower();
			powerText.text = "ちから(" + result.getPower() + ")";
			wise.value = result.getWise();
			wiseText.text = "賢さ(" + result.getWise() + ")";
			hit.value = result.getHit();
			hitText.text = "命中(" + result.getHit() + ")";
			avoid.value = result.getAvoid();
			avoidText.text = "回避(" + result.getAvoid() + ")";
			deffence.value = result.getDeffence();
			deffenceText.text = "防御(" + result.getDeffence() + ")";
			isDisp = true;
			dispStatusButtonText.text = "閉じる";
		}
	}
}
