using UnityEngine;
using System.Collections;

/**
 * モンスターの共通挙動を制御する。
 * 固有の動きは個別で設定すること
 */
using UnityStandardAssets.CrossPlatformInput;


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

	public float charaSpeed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	protected Vector3 moveDirection = Vector3.zero;
	protected CharacterController controller;

	// Use this for initialization
	public void Start () {
		// 共通アニメーション
		animator = GetComponent<Animator>();
		// 牧場モード用
		targetPosition = GetRandomPositionOnLevel();
		// キャラコン
		controller = GetComponent<CharacterController>();
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

	/**
	 * 図鑑モードで使われる、モンスターの回転
	 */
	protected void rotateMonster() {
		if (Input.GetKey("right")) {
			transform.Rotate(0, 10, 0);
		}
		if (Input.GetKey ("left")) {
			transform.Rotate(0, -10, 0);
		}
	}

	/**
	 * minusDirectionには「-1」か「1」を指定。
	 * -1を指定すると、操作逆になる。
	 */
	protected void moveTraining(int minusDirection = 1) {
		// アニメーション
		if (CrossPlatformInputManager.GetAxisRaw ("Horizontal") > 0 || CrossPlatformInputManager.GetAxisRaw ("Vertical") > 0) {
			animator.SetBool("isWalk", true);
		} else {
			animator.SetBool("isWalk", false);
		}

		// 向き
		transform.Rotate(0, CrossPlatformInputManager.GetAxisRaw("Horizontal"), 0);

		// 移動
		moveDirection = new Vector3 (minusDirection * CrossPlatformInputManager.GetAxisRaw("Horizontal"), 0, minusDirection * CrossPlatformInputManager.GetAxisRaw("Vertical"));
		moveDirection = transform.TransformDirection (moveDirection);
		moveDirection *= charaSpeed;
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}
}