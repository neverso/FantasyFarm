using Photon;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PhotonSystem : Photon.PunBehaviour {

	public Text nickNameText;

	void Start() {
		//マスターサーバーへ接続
		PhotonNetwork.ConnectUsingSettings("v0.1");
	}

	// サーバーが使用可能であるとき、クライアントが認証前に呼び出されます。
	void OnConnectedToPhoton(){
		Debug.Log ("OnConnectedToPhoton");
	}

	void OnConnectedToMaster() {
		Debug.Log ("OnConnectedToMaster");
	}

	/// マスターサーバーのロビーに入るに呼び出されます。
	void OnJoinedLobby() {
		Debug.Log("ロビーに入室");
		//ランダムにルームへ参加
		PhotonNetwork.JoinRandomRoom();
	}

	/// 部屋に入るとき呼ばれます。
	/// これは参加する際だけでなく作成する際も含みます。
	void OnJoinedRoom() {
		Debug.Log("部屋に入室");
		dispBattleMonster (getMyMonstername());
	}

	/// JoinRandom()の入室が失敗した場合に後に呼び出されます。
	void OnPhotonRandomJoinFailed() {
		Debug.Log("部屋入室失敗");
		//名前のないルームを作成
		PhotonNetwork.CreateRoom(null);
	}

	void OnPhotonCreateRoomFailed() {
		Debug.Log ("OnPhotonCreateRoomFailed");
	}

	// 接続後にPhotonサーバーとの通信が失敗した場合に呼ばれます。
	void OnConnectionFail(DisconnectCause cause){
		Debug.Log ("OnConnectionFail");
	}

	// フォトンから接続を切った時
	void OnDisconnectedFromPhoton(){
		Debug.Log ("OnDisconnectedFromPhoton");
	}
	// フォトンに繋げなかった時
	void OnFailedToConnectToPhoton(DisconnectCause cause){
		Debug.Log ("OnFailedToConnectToPhoton");
	}

	// 同時接続の最大数に達した際に呼ばれます。
	void OnPhotonMaxCccuReached(){
		Debug.Log ("OnPhotonMaxCccuReached");
	}

	// カスタム認証が失敗した際に呼ばれます
	void OnCustomAuthenticationFailed(string error){
		Debug.Log ("OnCustomAuthenticationFailed");
	}



	//////// ここまでphoton系のコールバック //////////
	Entity.Monsters getMyMonstername ()
	{
		Database.MonstersTable mt = new Database.MonstersTable ();
		//		Entity.MyMonster entity = mmt.selectMyMoster (PlayerPrefs.GetInt(Const.Const.nowMyMonsterID, 1));
		Entity.Monsters entity = mt.selectMosterByType (Random.Range (1, Const.Const.charactors.Count));
		return entity;
	}

	void dispBattleMonster (Entity.Monsters myMonster)
	{
		string prefabName = Const.Const.charactors[myMonster.getName()];
		nickNameText.text = myMonster.getDispname();
		GameObject prefab = (GameObject)Resources.Load (prefabName);
		// プレハブからインスタンスを生成
		GameObject createdObj = (GameObject)PhotonNetwork.Instantiate (prefabName, new Vector3(0, 0, 0), Quaternion.Euler(0, 180, 0), 0);
		createdObj.GetComponent<Common>().isBattle = true;
	}
}