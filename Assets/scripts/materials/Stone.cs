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
		if (collision.gameObject.tag.Equals("enemy") || collision.gameObject.tag.Equals("terrain")) {
			if (collision.gameObject.tag.Equals("enemy")) {
				GameObject prefab = (GameObject)Resources.Load ("prefabs/BirthAction");
				Instantiate (prefab, new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z), Quaternion.Euler(0, 180, 0));
				Destroy(collision.gameObject);
			}
			Destroy(gameObject);
		}
	}
}
