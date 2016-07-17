using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using System;

public class UnityAdd : MonoBehaviour {

	public static int birthMonsterIndex = 0;
	public static bool moveBirthSceneFlg = false;

	void Awake()
	{   
		// まずはAwake()内で、初期化をします。先ほどのゲームIDを入力。
		Advertisement.Initialize ("1064758");
	}

	void Start () {
		birthMonsterIndex = 0;
		moveBirthSceneFlg = false;
	}
	
	public void ShowAd() {
		using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
				activity.Call("setListener", new ActivityListener());
				activity.Call ("readQRCode", activity);
			}
		}

//		// 広告再生の準備ができているか確認。
//		if (Advertisement.IsReady ()) {
//			// 準備ができていたら、広告再生。
//			Advertisement.Show ();
//			// birthSceneに遷移
//			new SceneManager ().showBirthScene ();
//		}
	}

	// Update is called once per frame
	void Update () {
		if (moveBirthSceneFlg) {
			PlayerPrefs.SetInt (Const.Const.birthMonsterID, birthMonsterIndex);
			UnityAdd.moveBirthSceneFlg = false;
			new SceneManager ().showBirthScene ();
		}
	}

	public class ActivityListener : AndroidJavaProxy
	{
		public ActivityListener()
			: base("palcom.fantasyfarmunitylibrary.QRCodeListener")
		{
		}

		public void onRestart()
		{
		}

		public void onStart()
		{
		}

		public void onResume()
		{
		}

		public void onPause()
		{
		}

		public void onStop()
		{
		}

		/**
		 * dataにバーコードで読み取った結果テキストが返る
		 */
		public void onActivityResult(int requestCode, int resultCode, string data)
		{
			if (data == null || data.Length != 13) {
				AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
				AndroidJavaObject activity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
				activity.Call ("runOnUiThread", new AndroidJavaRunnable (() => {
					AndroidJavaObject alertDialogBuilder = new AndroidJavaObject ("android.app.AlertDialog$Builder", activity);
					alertDialogBuilder.Call<AndroidJavaObject> ("setMessage", "読み取れませんでした。");
					alertDialogBuilder.Call<AndroidJavaObject> ("setCancelable", true);
					alertDialogBuilder.Call<AndroidJavaObject> ("setPositiveButton", "OK", null);
					AndroidJavaObject dialog = alertDialogBuilder.Call<AndroidJavaObject> ("create");
					dialog.Call ("show");
				}));
			}

			string countryCode = data.Substring (0, 2);
			string makerCode = data.Substring (2, 5);
			string productCode = data.Substring (7, 5);
			char[] productCharArray = productCode.ToCharArray ();
			int[] productCodeIntArray = new int[productCharArray.Length + 1];
			int i = 0;
			foreach (char c in productCharArray) {
				int code = (int)Char.GetNumericValue (c);
				productCodeIntArray [i] = code;
				i++;
			}
			int ret = 0;
			foreach (int code in productCodeIntArray) {
				ret += code;
			}
			// PlayerPrefsにバーコードの結果値を保存し、birthsceneに使用する
			int birthMonsterIndex = 0;
			if (ret % 10 < Const.Const.charactors.Count) {
				birthMonsterIndex = ret % 10;
			}
			UnityAdd.birthMonsterIndex = birthMonsterIndex;
			UnityAdd.moveBirthSceneFlg = true;
		}
	}
}
