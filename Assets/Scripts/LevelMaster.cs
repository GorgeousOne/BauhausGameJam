using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LevelMaster : MonoBehaviour
{
    public int Score;
    int Level;
	GameObject Player;
    GameObject[] DinoCollection;

    // Start is called before the first frame update
    void Start()
    {
		Player = GameObject.FindWithTag("Player");
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
