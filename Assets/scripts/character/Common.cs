using UnityEngine;
using System.Collections;

/**
 * モンスターの共通挙動を制御する。
 * 固有の動きは個別で設定すること
 */
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;


public class Common : Photon.MonoBehaviour {

	// 牧場モードかどうか
	public bool isFarm = false;
	// 図鑑モードかどうか
	public bool isBook = false;
	// 対戦モードかどうか
	public bool isBattle = false;

	// 移動スピード
	public float speed = 0.5f;
	// 回転速度
	public float rotationSmooth = 1f;
	// 牧場内のランダム移動の位置
	private Vector3 targetPosition;
	//
	public float levelSize = 7f;
	private float changeTargetSqrDistance = 10f;
	public float rotateSpeed = 10f;

	// タイム処理
	public float timeOut = 1;
	private float timeElapsed;

	// アニメーション
	protected Animator animator;

	public float charaSpeed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	protected Vector3 moveDirection = Vector3.zero;
	protected CharacterController controller;

	// 発射玉のスピード、位置
	public int mergin_y = 3;
	public int mergin_z = 1;
	public int fireSpeed = 1500;

	// ライフ
	private int life = 5;
	// ライフバー
	private Slider lifeBar;

	// Use this for initialization
	public void Start () {
		// 共通アニメーション
		animator = GetComponent<Animator>();
		// 牧場モード用
		targetPosition = GetRandomPositionOnLevel();
		// キャラコン
		controller = GetComponent<CharacterController>();
		if (GameObject.Find ("life-bar") != null) {
			lifeBar = GameObject.Find("life-bar").GetComponent<Slider>();
		}

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
		return new Vector3(Random.Range(0, levelSize), 0, Random.Range(0, levelSize));
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
			animator.SetBool ("isWalk", true);
		} else {
			animator.SetBool ("isWalk", false);
		}

		// 向き
		if (Application.platform == RuntimePlatform.Android) {
			if (CrossPlatformInputManager.GetAxisRaw ("Horizontal") > 0) {
				transform.Rotate (0, CrossPlatformInputManager.GetAxisRaw ("Horizontal") + 2, 0);
			} else if (CrossPlatformInputManager.GetAxisRaw ("Horizontal") < 0) {
				transform.Rotate (0, CrossPlatformInputManager.GetAxisRaw ("Horizontal") - 2, 0);
			}
		} else {
			transform.Rotate (0, CrossPlatformInputManager.GetAxisRaw ("Horizontal"), 0);
		}

		// 移動
		moveDirection = new Vector3 (minusDirection * CrossPlatformInputManager.GetAxisRaw ("Horizontal"), 0, minusDirection * CrossPlatformInputManager.GetAxisRaw ("Vertical"));
		moveDirection = transform.TransformDirection (moveDirection);
		moveDirection *= charaSpeed;
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move (moveDirection * Time.deltaTime);
	}

	////////////////////
	///// battle ////
	////////////////////

	protected void moveBattle(int minusDirection = 1) {
		if (life <= 0) {
			explosion ();
		}
		speed = 6.0F;
		jumpSpeed = 8.0F;
		rotateSpeed=10f;
		gravity = 20.0F;
		// 攻撃間隔に使用
		timeElapsed += Time.deltaTime;


		if (photonView.isMine) { // 自分が作ったキャラクターなら、動く
			CameraAxisControl();
			jumpControl();
			attachMove();
			attachRotation();
			attack ();
		}
	}

	//カメラ軸に沿った移動コントロール
	void  CameraAxisControl(){
		if (controller.isGrounded) {
			Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
			Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);

			moveDirection = Input.GetAxis("Horizontal")*right + Input.GetAxis("Vertical")*forward;
			moveDirection *= speed;
		}
	}

	//標準的なジャンプコントロール
	void jumpControl (){
		if (Input.GetButton("Jump"))
			moveDirection.y = jumpSpeed;
	}

	//移動処理 
	void attachMove (){
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}
		
	//キャラクターを進行方向へ向ける処理 
	void attachRotation(){
		var moveDirectionYzero = moveDirection;
		moveDirectionYzero.y=0;

		//ベクトルの２乗の長さを返しそれが0.001以上なら方向を変える（０に近い数字なら方向を変えない） 
		if (moveDirectionYzero.sqrMagnitude > 0.001){

			//２点の角度をなだらかに繋げながら回転していく処理（stepがその変化するスピード） 
			float    step = rotateSpeed*Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(transform.forward,moveDirectionYzero,step,0f);

			transform.rotation = Quaternion.LookRotation(newDir);
		}
	}

	void attack() {
		if(Input.GetKey("f")) {
			if(timeElapsed >= timeOut) {
				// Stoneの生成
				GameObject prefab = (GameObject)Resources.Load ("prefabs/stone");
				// キャラクターの取得
				GameObject createdObj = (GameObject)PhotonNetwork.Instantiate ("prefabs/stone", new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y + mergin_y, gameObject.transform.position.z + mergin_z), Quaternion.Euler (0, 0, 0), 0);
				Vector3 force = gameObject.transform.forward * fireSpeed;
				// Rigidbodyに力を加えて発射
				createdObj.GetComponent<Rigidbody>().AddForce (force);

				timeElapsed = 0.0f;
			}
		}
	}

	// 衝突があったさいに呼ばれる
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.gameObject.name.Equals("gameover-plane")) {
			life = 0;
			lifeBar.value = 0;
			explosion ();
		}
		if (!hit.gameObject.name.Equals("Plane")) {
			Debug.Log (hit.gameObject.name);
		}
			
		if (hit.gameObject.name.Equals("stone") || hit.gameObject.name.Equals("stone(Clone)") && timeElapsed >= timeOut) {
			Debug.Log (hit.gameObject.name);
			GameObject prefab = (GameObject)Resources.Load ("prefabs/ExplosionMobile");
			PhotonNetwork.Instantiate ("prefabs/ExplosionMobile", new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.Euler(0, 180, 0), 0);
			life--;
			lifeBar.value = life;
			timeElapsed = 0.0f;
		}
	}

	// 死亡処理
	private void explosion() {
		GameObject prefab = (GameObject)Resources.Load ("prefabs/BirthAction");
		PhotonNetwork.Instantiate ("prefabs/BirthAction", new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.Euler(0, 180, 0), 0);
		Destroy (gameObject);
	}
}