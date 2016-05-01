using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MonsterListSelected : MonoBehaviour {

	public Text monsterButtonName;
	private Text description;
	private GameObject createdObj = null;

	// Use this for initialization
	void Start () {
		description = (Text)GameObject.Find ("description").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onSelectedMonster() {
		Debug.Log(monsterButtonName.text);
		GameObject oldPrefab = GameObject.FindGameObjectWithTag("monster");
		if (oldPrefab != null) {
			GameObject.Destroy(oldPrefab);
		}
		string prefabName = Const.Const.charactors[Const.Const.charactorNames[monsterButtonName.text]];
		GameObject prefab = (GameObject)Resources.Load (prefabName);
		// プレハブからインスタンスを生成
		if (Const.Const.charactorNames[monsterButtonName.text].Equals ("crayslug")) {
			createdObj = (GameObject)Instantiate (prefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
			setBookFlg(createdObj);
		} else {
			createdObj = (GameObject)Instantiate (prefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 180, 0));
			setBookFlg(createdObj);
		}

		// 説明文
		description.text = getDescription (Const.Const.charactorNames[monsterButtonName.text]);
	}

	string getDescription (string name)
	{
		Database.MonstersTable mmt = new Database.MonstersTable ();
		Entity.Monsters entity = mmt.selectMosterInfo (name);
		return entity.getDescription ();
	}

	void setBookFlg (GameObject createdObj)
	{
		createdObj.GetComponent<Common> ().isBook = true;
	}

	// ドラッグイベントリスナー(https://github.com/naichilab/Unity-TouchManager)
	void OnEnable ()
	{
		if (TouchManager.Instance != null) {
			TouchManager.Instance.Drag += OnDrag;
		}
	}
	void OnDisable ()
	{
		if (TouchManager.Instance != null) {
			TouchManager.Instance.Drag -= OnDrag;
		}
	}
	void OnDrag (object sender, CustomInputEventArgs e)
	{
		string text = string.Format ("OnSwipe Pos[{0},{1}] Move[{2},{3}]", new object[] {
			e.Input.ScreenPosition.x.ToString ("0"),
			e.Input.ScreenPosition.y.ToString ("0"),
			e.Input.DeltaPosition.x.ToString ("0"),
			e.Input.DeltaPosition.y.ToString ("0")
		});
		Debug.Log (text);
		if (createdObj != null) {
			createdObj.transform.Rotate (new Vector3(0, -1 * (e.Input.DeltaPosition.y * 2), 0f));
		}
	}
	// ここまで
}
