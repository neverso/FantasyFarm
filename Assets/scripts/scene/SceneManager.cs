using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	// ファームに遷移
	public void startGame() {
		Application.LoadLevel("farm");
	}
	// スタート画面に遷移
	public void quitGame() {
		Application.LoadLevel("start");
	}
	// モンスター図鑑に遷移
	public void showMonsterBook(){
		Application.LoadLevel ("monster_book");
	}
	// モンスター再生に遷移
	public void showBirthScene ()
	{
		Application.LoadLevel ("birth");
	}
	// トレーニング画面に遷移
	public void showTrainingScene() {
		Application.LoadLevel ("training");
	}
}
