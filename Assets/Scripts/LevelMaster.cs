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
    int Level = 1;
    List<GameObject> SpawnQueue = new List<GameObject>();



    // Start is called before the first frame update
    void Start()
    {
		Player = GameObject.FindWithTag("Player");
        Player.GetComponent<PlayerController>().OnHealthChange.AddListener(_checkForDeath);
        playerController = Player.GetComponent<PlayerController>();
        
        highscore = ScoreBar.GetComponent<Highscore>();
        // Assessment Preparation
        
        queueDinos();
        highscore.UpdateScore(0);
	}

    // Update is called once per frame
    void Update()
    {
        if(SpawnMetronom() && checkLoad() && SpawnQueue.Count != 0){
            SpawnDino();
        }
        if(SpawnQueue.Count == 0){
            queueDinos();
            Level++;
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

    public static List<T> Shuffle<T>(List<T> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            T temp = _list[i];
            int randomIndex = Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }

        return _list;
    }

    void ShuffleCollection()
    {
        GameObject[] newDinoOrder = new GameObject[4];
        List<int> Indexes = new List<int> {0, 1, 2, 3};
        Shuffle<int>(Indexes);
        for(int i = 0;i < Indexes.Count; i++){
            int randIndex = Random.Range(0,Indexes.Count); 
            newDinoOrder[i] = DinoCollection[Indexes[i]];
        }
        DinoCollection = newDinoOrder;
    }

    void queueDinos()
    {
        for(int i = 0; i < Level; i++)
        {
            ShuffleCollection();
            foreach (GameObject Dino in DinoCollection){
                SpawnQueue.Add(Dino);
            }
        }
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
        GameObject Dino = SpawnQueue[0];
        GameObject Doni = Instantiate(Dino, _findSuitableSpawn(), Quaternion.identity);
        Doni.GetComponent<DinoAI>().OnDeath.AddListener(AddtoScore);
        SpawnQueue.RemoveAt(0);
    }

}
