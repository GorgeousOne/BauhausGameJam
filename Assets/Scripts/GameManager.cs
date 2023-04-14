using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager Singleton;
	
	public enum Scenes {
		MainMenu,
		Game
	}

	private Scenes? _currentLoadedScene;
	
	private void Awake() {
		Singleton = this;
		//create player
		LoadScene(Scenes.MainMenu);
		Debug.Log(Singleton);
	}

	public void LoadScene(Scenes newScene) {
		if (_currentLoadedScene != null) {
			SceneManager.UnloadSceneAsync(_currentLoadedScene.ToString());
		}
		SceneManager.LoadSceneAsync(newScene.ToString(), LoadSceneMode.Additive);
		Debug.Log("VAR");
		_currentLoadedScene = newScene;
	}
}
