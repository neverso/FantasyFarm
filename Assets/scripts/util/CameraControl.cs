using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	
	private GameObject player = null;
	public static bool isReady = false;

	void Start () {
		isReady = false;
	}
	void Update() {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("monster");
		}
	}
	
	void LateUpdate () {
		if (player != null && isReady) {
			transform.parent = player.transform;
			isReady = false;
		}
	}
}