using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LevelMaster : MonoBehaviour
{
    // Resources
	GameObject Player;
    PlayerController playerController;
    public GameObject[] DinoCollection;
    public GameObject[] DinoBossCollection;
    public GameObject[] SpawnLocations;
    enum GameState{
        AssessmentPhase,
        LevelPhase,
        BossPhase,
        RestingPhase
    }
    float _metronomTimer = 0;
    public float MetronomSpeed = 10;

    // Active Level Logic
    public int Score;
    int Level;
    List<GameObject> SpawnQueue = new List<GameObject>();
    public List<GameObject> InLevel= new List<GameObject>();
    GameState gameState;


    //Assesment
    int _assesmentDepth = 2;
    public List<List<float>> assessmentKillTime = new List<List<float>>();
    public List<List<int>> assessmentDamage = new List<List<int>>();


    // Start is called before the first frame update
    void Start()
    {
		Player = GameObject.FindWithTag("Player");
        playerController = Player.GetComponent<PlayerController>();
        
        // Assessment Preparation
        ShuffleCollection();
        queueAssessmentDinos();
	}

    // Update is called once per frame
    void Update()
    {
        switch(gameState){
            case GameState.AssessmentPhase:
                if((checkLoad() && SpawnMetronom()) && SpawnQueue.Count != 0){
                    SpawnDino();
                }
                if(SpawnQueue.Count == 0){
                    gameState = GameState.LevelPhase;
                }
                break;
            case GameState.LevelPhase:
                break;
            case GameState.BossPhase:
                break;
            case GameState.RestingPhase:
                break;
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
        GameObject[] newBossOrder = new GameObject[4];

        List<int> Indexes = new List<int> {0, 1, 2, 3};
        Shuffle<int>(Indexes);
        for(int i = 0;i < Indexes.Count; i++){
            int randIndex = Random.Range(0,Indexes.Count); 
            newDinoOrder[i] = DinoCollection[Indexes[i]];
            newBossOrder[i] = DinoBossCollection[Indexes[i]];
        }
        DinoCollection = newDinoOrder;
        DinoBossCollection = newBossOrder;
    }
    void queueAssessmentDinos()
    {
        for(int i = 0; i < _assesmentDepth; i++)
        {
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
    void SpawnDino(){
        GameObject Dino = SpawnQueue[0];
        InLevel.Add(Dino);
        SpawnQueue.RemoveAt(0);
        Instantiate(Dino, _findSuitableSpawn(), Quaternion.identity);
    }
    public int GetFeedbackIndex(GameObject UnkownDinoRace){
        for(int i = 0; i < 4; i++){
            if(DinoCollection[i].GetType() == UnkownDinoRace.GetType()){
                Debug.Log(i);
                return i;
            }
        }
        return 3;
    }
}
