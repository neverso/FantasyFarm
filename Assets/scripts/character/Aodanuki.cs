using UnityEngine;
using System.Collections;

public class Aodanuki : Common {

	public void Update() {
		if (isFarm) {
			moveRandom ();
		} else if (isBook) {
			rotateMonster();
		} else {
			moveTraining();
		}
	}
}
