using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LevelMaster : MonoBehaviour {
    [SerializeField] private Canvas gameOverScreen;
    // Resources
    GameObject Player;
    PlayerController playerController;
    public GameObject ScoreBar;
    Highscore highscore;
    public GameObject[] DinoCollection;
    public GameObject[] SpawnLocations;

    float _metronomTimer = 0;
    public float MetronomSpeed = 10;

    // Active Level Logic
    public int Score;

    // Start is called before the first frame update
    void Start()
    {
		Player = GameObject.FindWithTag("Player");
        Player.GetComponent<PlayerController>().OnHealthChange.AddListener(_checkForDeath);
        playerController = Player.GetComponent<PlayerController>();
        
        highscore = ScoreBar.GetComponent<Highscore>();
        // Assessment Preparation

        SpawnDino();
        highscore.UpdateScore(0);
	}

    // Update is called once per frame
    void Update()
    {
        if(SpawnMetronom()){
            SpawnDino();
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


    bool checkLoad() // Maybe better method
    {
        if (playerController.Health >= 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool SpawnMetronom()
    {
        if (_metronomTimer == 0){
            _metronomTimer = Time.time;
        }
        if ((_metronomTimer + MetronomSpeed) < Time.time)
        {
            _metronomTimer = 0;
            return true;
        }
        else
        {
            return false;
        }
    }
    void AddtoScore(int val){
        Score += val;
        highscore.UpdateScore(Score);
        Player.GetComponent<PlayerController>().AddHealth();
    }

    private void _checkForDeath(int health) {
        if (health != 0) {
            return;
        }
        gameOverScreen.gameObject.SetActive(true);
    }
    
    void SpawnDino(){
        GameObject Dino = DinoCollection[Random.Range(0,4)];
        GameObject Doni = Instantiate(Dino, _findSuitableSpawn(), Quaternion.identity);
        Doni.GetComponent<DinoAI>().OnDeath.AddListener(AddtoScore);
    }

}
