using UnityEngine;
using System.Collections;

public class CraySlug : Common {

	// Update is called once per frame
	void Update () {
		if (isFarm) {
			// TODO こいつだけ逆むくので、引数追加か・・
			moveRandom ();
		} else if (isBook) {
			rotateMonster();
		} else {
			if (Input.GetKey ("up")) {
				// こいつは前後が逆になってたので、+-入れ替える。
				transform.position -= transform.forward * 0.01f;
			} else if (Input.GetKey ("down")) {
				transform.position += transform.forward * 0.01f;
			}
			if (Input.GetKey("right")) {
				transform.Rotate(0, 10, 0);
			}
			if (Input.GetKey ("left")) {
				transform.Rotate(0, -10, 0);
			}
		}
	}
}
