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

    [Header("Health Settings")]

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
    [Header("Movementpattern 2 Settings")]
    [SerializeField] protected float speed = 10;
    [SerializeField] protected float accelerateTime = 0.2f;
    
    protected Rigidbody2D _rigid;
    public Vector2 _moveInput;
    
    public GameObject Target;

    protected Vector2 TargetVector2;
    float timer3;
    protected int movementStage2;
    public float targetVectorUpdatetime2 = 1;

    //Shooting
    [Header("InfectionSpreading Settings")]
    [SerializeField] private Projectile bulletPrefab;
    private float _lastShot;
    public float projectileSpawnDistance = 1;
    
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
                    break;
                }
                Timer = Timer + Time.deltaTime;
                break;
            
            case InfectionStage.Kawaii:
                MovementStage2();
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
                }
                break;

            case InfectionStage.Kawaii:
                if(infectionLevel >= KawaiiOverload)
                {
                    Stage = InfectionStage.Explode;
                    SpriteRenderer.sprite = sprites[2];
                }
                break;
        }
    }

    protected Vector2 getTargetDist() {
        return Target.transform.position - transform.position;
    }
    
    protected abstract void MovementStage1();

    void MovementStage2()
    {   
        
        switch(movementStage2) 
        {
            case 0: // Targeting stage
                TargetVector2 = new Vector2(Random.value-0.5f, Random.value-0.5f).normalized;
                movementStage2 = 1;
                timer3 = Time.time;
                break;
            case 1:
                _moveInput = TargetVector2.normalized;
                
                if(timer3+targetVectorUpdatetime2 < Time.time){
                    movementStage2 = 0;
                }
                break;
        }
    }

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
        }   
    }

    private void _shoot() {
        _lastShot = Time.time;
        Vector3 shootDirection = (mousePos - transform.position).normalized;
        Projectile bullet = Instantiate(bulletPrefab, gameObject.transform.position+shootDirection*projectileSpawnDistance, Quaternion.identity);
        bullet.SetShotDirection(shootDirection);
    }
}
