using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenuLogic : MonoBehaviour
{
	public void BackToMenu() {
		GameManager.Singleton.LoadScene(GameManager.Scenes.MainMenu);
	}
}
