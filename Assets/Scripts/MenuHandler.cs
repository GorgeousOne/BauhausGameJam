using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() {
        Debug.Log(GameManager.Singleton);
        GameManager.Singleton.LoadScene(GameManager.Scenes.Game);
    }
}
