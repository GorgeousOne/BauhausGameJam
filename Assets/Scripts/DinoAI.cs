using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DinoAI : MonoBehaviour
{
    enum InfectionStage{
        Dino,
        Transform,
        Kawaii,
        Explode
    }
    InfectionStage Stage = InfectionStage.Dino;
    public int infectionLevel = 0;
    public int Infected = 2;
    public int KawaiiOverload = 4;
    float Timer = 0.0f;
    public float tranformationTime = 5.0f;
    public float explosionTime = 5.0f;
        

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        switch(Stage) 
        {
            case InfectionStage.Dino:
                if(infectionLevel >= Infected)
                {
                    Stage = InfectionStage.Transform;
                    
                }

                Debug.Log("Dino");
                break;

            case InfectionStage.Transform:
                if (Timer > tranformationTime)
                {
                    Stage = InfectionStage.Kawaii;
                    Timer = 0;
                    break;
                }
                Timer = Timer + Time.deltaTime;

                Debug.Log("transform");
                break;

            case InfectionStage.Kawaii:
                if(infectionLevel >= KawaiiOverload)
                {
                    Stage = InfectionStage.Explode;
                }

                Debug.Log("kawaii");
                break;

            case InfectionStage.Explode:
                if (Timer > explosionTime)
                {
                    
                    // EXPLODE!!
                    Destroy(gameObject);
                }
                Timer = Timer + Time.deltaTime;
                
                Debug.Log("Explosion");
                break;
        }
    }

    void transformKawaii()
    {

    }
}
