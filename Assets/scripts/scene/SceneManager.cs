using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	public void startGame() {
		Application.LoadLevel("farm");
	}
	public void quitGame() {
		Application.LoadLevel("start");
	}
	public void showMonsterBook(){
		Application.LoadLevel ("monster_book");
	}

	public void showBirthScene ()
	{
		Application.LoadLevel ("birth");
	}
}
