using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DinoAI : MonoBehaviour
{
    // Fields
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

    // Sprites
    public Sprite[] sprites;

    // Components
    SpriteRenderer SpriteRenderer;
    
    [SerializeField] private float speed = 10;
    [SerializeField] private float accelerateTime = 0.2f;
    
    private GameInputs _gameInputs;
    private Rigidbody2D _rigid;
    public Vector2 _moveInput;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        _CalcMoveSpeed();
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
                    SpriteRenderer.sprite = sprites[0];
                    Debug.Log("transform");
                }

                Debug.Log("Dino");
                break;

            case InfectionStage.Transform:
                if (Timer > tranformationTime)
                {
                    Stage = InfectionStage.Kawaii;
                    Timer = 0;
                    SpriteRenderer.sprite = sprites[1];
                    Debug.Log("kawaii");
                    break;
                }
                Timer = Timer + Time.deltaTime;
                break;

            case InfectionStage.Kawaii:
                if(infectionLevel >= KawaiiOverload)
                {
                    Stage = InfectionStage.Explode;
                    SpriteRenderer.sprite = sprites[2];
                    Debug.Log("Explosion");
                }

                break;

            case InfectionStage.Explode:
                if (Timer > explosionTime)
                {
                    
                    // EXPLODE!!
                    Destroy(gameObject);
                }
                Timer = Timer + Time.deltaTime;

                break;
        }
    }

    void transformKawaii()
    {

    }

    private void _CalcMoveSpeed() {
        Vector2 newVelocity = _rigid.velocity;
		
        if (_moveInput.magnitude > 0.01) {
            newVelocity = GetAccelerated(_moveInput, newVelocity.magnitude);
        }
        else {
            newVelocity = GetDecelerated(newVelocity);
        }
        _rigid.velocity = newVelocity;
    }
	
    private Vector2 GetAccelerated(Vector2 headedDir, float currentSpeed) {
        float acceleration = Time.fixedDeltaTime / accelerateTime * speed ;
        Vector2 newSpeed = headedDir.normalized * (currentSpeed + acceleration);
        return Vector2.ClampMagnitude(newSpeed, speed);
    }
    
    private Vector2 GetDecelerated(Vector2 currentVel) {
        float deceleration = Time.fixedDeltaTime / accelerateTime * speed;
        float currSpeed = currentVel.magnitude;
        
        if (Mathf.Abs(currSpeed) < deceleration) {
            return Vector2.zero;
        }
        Vector2 newSpeed = currentVel.normalized * (currSpeed - deceleration);
        return Vector2.ClampMagnitude(newSpeed, speed);
    }
}
