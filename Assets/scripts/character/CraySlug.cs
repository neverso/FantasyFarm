using UnityEngine;
using System.Collections;

public class CraySlug : Common {

	// Update is called once per frame
	protected void Update () {
		if (isFarm) {
			moveRandom ();
		} else if (isBook) {
			rotateMonster();
		} else {
			float x =  Input.GetAxis("Horizontal");
			float z = Input.GetAxis("Vertical");
			
			Rigidbody rigidbody = GetComponent<Rigidbody>();
			
			// rigidbodyのx軸（横）とz軸（奥）に力を加える
			rigidbody.AddForce(x * 5, 0, z * 5);
		}
	}
}
