using UnityEngine;
using System.Collections;

public class CraySlug : Common {

	public void Update() {
		if (isFarm) {
			moveRandom ();
		} else if (isBook) {
			rotateMonster();
		} else {
			moveTraining(-1);
		}
	}
}
