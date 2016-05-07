using UnityEngine;
using System.Collections;

public class Stone : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log (collision.gameObject.tag);
		if (collision.gameObject.tag.Equals("enemy") || collision.gameObject.tag.Equals("terrain")) {
			Destroy(gameObject);
		}
	}
}
