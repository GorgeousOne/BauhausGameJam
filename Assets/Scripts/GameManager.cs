using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager Singleton;
	
	public enum Scenes {
		MainMenu
	}

	private Scenes? _currentLoadedScene;
	
	private void Awake() {
		Singleton = this;
		//create player
		LoadScene(Scenes.MainMenu);
	}

	public void LoadScene(Scenes newScenes) {
		if (_currentLoadedScene != null) {
			SceneManager.UnloadSceneAsync(_currentLoadedScene.ToString());
		}
		SceneManager.LoadSceneAsync(newScenes.ToString());
		_currentLoadedScene = newScenes;
	}	
}
