using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class UnityAdd : MonoBehaviour {

	void Awake()
	{   
		// まずはAwake()内で、初期化をします。先ほどのゲームIDを入力。
		Advertisement.Initialize ("1064758");
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
			AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
			activity.Call ("runOnUiThread", new AndroidJavaRunnable (() => {
				AndroidJavaObject alertDialogBuilder = new AndroidJavaObject ("android.app.AlertDialog$Builder", activity);
				alertDialogBuilder.Call<AndroidJavaObject> ("setMessage", data);
				alertDialogBuilder.Call<AndroidJavaObject> ("setCancelable", true);
				alertDialogBuilder.Call<AndroidJavaObject> ("setPositiveButton", "OK", null);
				AndroidJavaObject dialog = alertDialogBuilder.Call<AndroidJavaObject> ("create");
				dialog.Call ("show");
			}));
		}
	}
}
