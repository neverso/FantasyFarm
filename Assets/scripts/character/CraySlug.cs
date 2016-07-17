using UnityEngine;
using System.Collections;

public class CraySlug : Common {

	public void Update() {
		if (isFarm) {
			moveRandom ();
		} else if (isBook) {
			rotateMonster ();
		} else if (isBattle) {
			moveBattle ();
		} else {
			moveTraining(-1);
		}
	}
}
