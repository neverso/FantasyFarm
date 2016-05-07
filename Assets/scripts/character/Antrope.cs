using UnityEngine;
using System.Collections;

public class Antrope : Common {
	
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
