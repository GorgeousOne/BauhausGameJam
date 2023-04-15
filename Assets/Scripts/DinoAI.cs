using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DinoAI : MonoBehaviour
{
    // Fields
    public enum InfectionStage{
        Dino,
        Transform,
        Kawaii,
        Explode
    }

    protected InfectionStage Stage = InfectionStage.Dino;
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
    

    //Movement
    [SerializeField] protected float speed = 10;
    [SerializeField] protected float accelerateTime = 0.2f;
    
    protected Rigidbody2D _rigid;
    public Vector2 _moveInput;
    
    public GameObject Target;

    
    // Start is called before the first frame update
    protected void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        Target = GameObject.FindWithTag("Player");
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
                MovementStage1();
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

    public void AddInfection(int levels) 
    {
        infectionLevel += levels;

        switch(Stage) 
        {
            case InfectionStage.Dino:
                if(infectionLevel >= Infected)
                {
                    Stage = InfectionStage.Transform;
                    SpriteRenderer.sprite = sprites[0];
                    Debug.Log("transform");
                }
                break;

            case InfectionStage.Kawaii:
                if(infectionLevel >= KawaiiOverload)
                {
                    Stage = InfectionStage.Explode;
                    SpriteRenderer.sprite = sprites[2];
                    Debug.Log("Explosion");
                }
                break;
        }
    }

    protected Vector2 getTargetDist() {
        return Target.transform.position - transform.position;
    }
    
    protected abstract void MovementStage1();

    protected void transformKawaii()
    {
        
    }

    protected void _CalcMoveSpeed() {
        Vector2 newVelocity = _rigid.velocity;
		
        if (_moveInput.magnitude > 0.01) {
            newVelocity = GetAccelerated(_moveInput, newVelocity.magnitude);
        }
        else {
            newVelocity = GetDecelerated(newVelocity);
        }
        _rigid.velocity = newVelocity;
    }
	
    protected Vector2 GetAccelerated(Vector2 headedDir, float currentSpeed) {
        float acceleration = Time.fixedDeltaTime / accelerateTime * speed ;
        Vector2 newSpeed = headedDir.normalized * (currentSpeed + acceleration);
        return Vector2.ClampMagnitude(newSpeed, speed);
    }
    
    protected Vector2 GetDecelerated(Vector2 currentVel) {
        float deceleration = Time.fixedDeltaTime / accelerateTime * speed;
        float currSpeed = currentVel.magnitude;
        
        if (Mathf.Abs(currSpeed) < deceleration) {
            return Vector2.zero;
        }
        Vector2 newSpeed = currentVel.normalized * (currSpeed - deceleration);
        return Vector2.ClampMagnitude(newSpeed, speed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Virus")
        {
            AddInfection(1);
            Debug.Log("hit");
        }   
    }
}
