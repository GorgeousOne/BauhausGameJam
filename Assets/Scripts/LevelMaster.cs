using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LevelMaster : MonoBehaviour
{
    public int Score;
    int Level;
	GameObject Player;
    public GameObject[] DinoCollection;
    public GameObject[] SpawnLocations;

    float Spawntimer = 0;

    // Start is called before the first frame update
    void Start()
    {
		Player = GameObject.FindWithTag("Player");
	}

    // Update is called once per frame
    void Update()
    {
        if (Spawntimer == 0 || Spawntimer + 10 < Time.time)
        {
            Instantiate(DinoCollection[Random.Range(0,4)], _findSuitableSpawn(), Quaternion.identity);
            Spawntimer = Time.time;
        }
    }

    Vector3 _findSuitableSpawn()
    {
        Vector3 furthestSpawn = SpawnLocations[0].transform.position;
        float furthestSpawndist = 0.0f;
        foreach(GameObject Spawn in SpawnLocations)
        {
            float distance = (Player.transform.position - Spawn.transform.position).magnitude;
            if (distance > furthestSpawndist)
            {
                furthestSpawndist = distance;
                furthestSpawn = Spawn.transform.position;
            }
        }
        return furthestSpawn;
    }
}
