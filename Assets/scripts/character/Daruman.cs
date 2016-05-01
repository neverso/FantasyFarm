using UnityEngine;
using System.Collections;

public class Daruman : Common {
	
	private void Update() {
		if (isFarm) {
			moveRandom ();
		} else if (isBook) {
			rotateMonster();
		} else {
			if (Input.GetKey("up")) {
				transform.position += transform.forward * 0.01f;
				animator.SetBool("isWalk", true);
			} else if (Input.GetKey("down")) {
				transform.position -= transform.forward * 0.01f;
				animator.SetBool ("isWalk", true);
			} else {
				animator.SetBool("isWalk", false);
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
