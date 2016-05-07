using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour {

	public static bool isReadyState = false;
	public int mergin_y = 0;
	public int mergin_z = 0;
	public int speed = 0;

	public void start() {
		isReadyState = false;
	}
	public void update() {
	}
	public void onAttack() {
		if (isReadyState) {
			// Stoneの生成
			GameObject prefab = (GameObject)Resources.Load ("prefabs/stone");
			// キャラクターの取得
			GameObject player = GameObject.FindGameObjectWithTag("monster");
			GameObject createdObj = (GameObject)Instantiate (prefab, new Vector3 (player.transform.position.x, player.transform.position.y + mergin_y, player.transform.position.z + mergin_z), Quaternion.Euler (0, 0, 0));
			Vector3 force = player.transform.forward * speed;
			// Rigidbodyに力を加えて発射
			createdObj.GetComponent<Rigidbody>().AddForce (force);
		}
	}
}
