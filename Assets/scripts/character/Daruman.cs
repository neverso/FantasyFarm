using UnityEngine;
using System.Collections;

public class Daruman : Common {
	
	public void Update() {
		if (isFarm) {
			moveRandom ();
		} else if (isBook) {
			rotateMonster ();
		} else if (isBattle) {
			moveBattle ();
		} else {
			moveTraining();
		}
	}
}
