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

    public GameObject[] SpawnLocations;

    float _metronomTimer = 0;
    public float MetronomSpeed = 5;

    // Active Level Logic
    public int Score;
    int Level = 1;
    List<GameObject> SpawnQueue = new List<GameObject>();
    public List<GameObject> InLevel= new List<GameObject>();



    //Assesment
    int _assesmentDepth = 2;


    // Start is called before the first frame update
    void Start()
    {
		Player = GameObject.FindWithTag("Player");
        playerController = Player.GetComponent<PlayerController>();
        
	}

    // Update is called once per frame
    void Update()
    {
        if(SpawnQueue.Count == 0){
            LoadLevelDinos();
            Level++;
        }
        if(SpawnMetronom() && checkLoad()){
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
    void LoadLevelDinos(){
        for (int i = 0; i < Level; i++){
            ShuffleCollection();
            queueAssessmentDinos();
        }
    }

}
