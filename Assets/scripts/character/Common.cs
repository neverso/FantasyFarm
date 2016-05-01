using UnityEngine;
using System.Collections;

/**
 * モンスターの共通挙動を制御する。
 * 固有の動きは個別で設定すること
 */
public class Common : MonoBehaviour {

	// 牧場モードかどうか
	public bool isFarm = false;
	// 図鑑モードかどうか
	public bool isBook = false;

	// 移動スピード
	public float speed = 0.5f;
	// 回転速度
	public float rotationSmooth = 1f;
	// 牧場内のランダム移動の位置
	private Vector3 targetPosition;
	//
	public float levelSize = 7f;
	private float changeTargetSqrDistance = 10f;

	// アニメーション
	protected Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		targetPosition = GetRandomPositionOnLevel();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected void moveRandom() {
		// 目標地点との距離が小さければ、次のランダムな目標地点を設定する
		float sqrDistanceToTarget = Vector3.SqrMagnitude(transform.position - targetPosition);
		if (sqrDistanceToTarget < changeTargetSqrDistance)
		{
			animator.SetBool ("isWalk", false);
			targetPosition = GetRandomPositionOnLevel();
		}
		
		// 目標地点の方向を向く
		Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmooth);
		
		// 前方に進む
		animator.SetBool ("isWalk", true);
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}
	
	public Vector3 GetRandomPositionOnLevel(){
		return new Vector3(Random.Range(-levelSize, levelSize), 0, Random.Range(-levelSize, levelSize));
	}

	protected void rotateMonster() {
		if (Input.GetKey("right")) {
			transform.Rotate(0, 10, 0);
		}
		if (Input.GetKey ("left")) {
			transform.Rotate(0, -10, 0);
		}
	}
}