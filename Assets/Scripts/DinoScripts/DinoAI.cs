using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DinoAI : MonoBehaviour
{
    public enum InfectionStage{
        Dino,
        Transform,
        Kawaii,
        Explode
    }

  
    protected InfectionStage Stage = InfectionStage.Dino;
	int infectionLevel = 0;
	[Header("Health Settings")]
	public int Infected = 2;
    public int KawaiiOverload = 4;

	// Kawaii Transformations
	[Header("Kawaii Transformation Settings")]
	float Timer = 0.0f;
    public float tranformationTime = 5.0f;
    

	// Sprites
	[Header("Sprite Settings")]
	public Sprite[] sprites;

	private int _spriteState = 0;
    private float _spriteTimer = 0;
    public float spriteChangingTime = 0.5f;
    public int _spriteStage = 0;

    // Components
    SpriteRenderer SpriteRenderer;
    CapsuleCollider2D CapsuleCollider;
    CircleCollider2D ExplosionCollider;
	protected Rigidbody2D _rigid;

	//Movement
	[Header("Movement Settings")]
    [SerializeField] protected float speed = 10;
    [SerializeField] protected float accelerateTime = 0.2f;
    
	[HideInInspector] public Vector2 _moveInput;

	[HideInInspector] public GameObject Target;

    protected Vector2 TargetVector2;
    float timer3;
    protected int movementStage2;
    public float targetVectorUpdatetime2 = 1;

	//Shooting
	[Header("InfectionSpreading Settings")]
    [SerializeField] private Projectile bulletPrefab;
    private float _lastShot;
    public float projectileSpawnDistance = 1;
    public float shootingIntervall = 1;

	// Explosion
	[Header("Explosion Settings")]
	public Sprite[] ExplosionFrames;
	public float explosionTime = 5.0f;
    bool exploding = false;



	// Start is called before the first frame update
	protected void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        ExplosionCollider = GetComponent<CircleCollider2D>();
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
                if (Timer == 0) 
                { 
                    Timer = Time.time;
					_spriteStage = 1;
				}
                
                if (Timer + tranformationTime/2 < Time.time)
                {
					_spriteStage = 2;
				}

				if (Timer + tranformationTime < Time.time)
				{
					Stage = InfectionStage.Kawaii;
					Timer = 0;
					SpriteRenderer.sprite = sprites[1];
					_spriteStage = 3;
					break;
				}
				break;
            
            case InfectionStage.Kawaii:
                MovementStage2();
                if(_lastShot+shootingIntervall < Time.time)
                {
                    _shoot();
                }
                break;

            case InfectionStage.Explode:
                if (Timer > explosionTime)
                {
                    Destroy(gameObject);
                }
                Timer = Timer + Time.deltaTime;
                
                if (Timer / explosionTime > 0.75f)
                {
                    SpriteRenderer.sprite = ExplosionFrames[2];
                }
				else if (Timer / explosionTime > 0.5f)
				{
					SpriteRenderer.sprite = ExplosionFrames[1];
				}
				else
				{
                    Debug.Log("ignite");
					SpriteRenderer.sprite = ExplosionFrames[0];
				}
				break;
        }

        //Sprite Update
        if (!exploding)
        {
			if (_spriteTimer + spriteChangingTime > Time.time)
			{
				_spriteTimer = Time.time;
				if (_spriteState == 0)
				{
					_spriteState = 1;
					SpriteRenderer.sprite = sprites[_spriteStage * 2 + _spriteState];
				}
				else
				{
					_spriteState = 0;
					SpriteRenderer.sprite = sprites[_spriteStage * 2 + _spriteState];
				}
			}

			if (_moveInput.x > 0) // find Better solution for collider flipping
			{
				SpriteRenderer.flipX = true;
				CapsuleCollider.offset = new Vector2(CapsuleCollider.offset.x * -1, CapsuleCollider.offset.y);
			}
			else
			{
				SpriteRenderer.flipX = false;
				CapsuleCollider.offset = new Vector2(CapsuleCollider.offset.x * -1, CapsuleCollider.offset.y);
			}
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
					_moveInput = new Vector2(0, 0);
				}
                break;

            case InfectionStage.Kawaii:
                if(infectionLevel >= KawaiiOverload)
                {
                    Stage = InfectionStage.Explode;
                    _explode();
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
        Vector3 shootDirection = TargetVector2;
        Projectile bullet = Instantiate(bulletPrefab, gameObject.transform.position+shootDirection*projectileSpawnDistance, Quaternion.identity);
        bullet.SetShotDirection(shootDirection);
    }

    private void _explode()
    {
        _moveInput = new Vector2(0, 0);
        CapsuleCollider.enabled = false;
        ExplosionCollider.enabled = true;
		// freeze Position
		exploding = true;
    }
}
