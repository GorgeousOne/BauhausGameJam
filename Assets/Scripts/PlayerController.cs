using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float speed = 10;
    [SerializeField] private float accelerateTime = 0.2f;
    
    private GameInputs _gameInputs;
    private bool _movedLastUpdate;
    private Rigidbody2D _rigid;

    public float Health = 4;
    CircleCollider2D CircleCollider2D;
	SpriteRenderer SpriteRenderer;

	[Header("Sprite Settings")]
	public Sprite[] sprites;

	private int _spriteState = 0;
	private float _spriteTimer = 0;
	public float spriteChangingTime = 0.5f;
	public int _spriteStage = 0;

    public enum PlayerStages
    {
        walk,
        shoot,
        gotHit,
        died
    }
    public PlayerStages playerStages = PlayerStages.walk;

    public float HitTimeout = 1;
    float HitTimer = 0;

	public float ShootTimeout = 1;
	float ShootTimer = 0;

	public float PoofDuration = 1;
	float PoofTimer = 0;
    public Sprite[] PoofSprites;

	//Sounds 
	AudioSource audioSource;

	void Awake() {
        _rigid = GetComponent<Rigidbody2D>();
        _gameInputs = new GameInputs();
        _gameInputs.Enable();
		SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		CircleCollider2D = gameObject.GetComponent<CircleCollider2D>();
		audioSource = gameObject.GetComponent<AudioSource>();
	}

     Vector2 _moveInput;
    
    void Update() {
		
        switch(playerStages)
        {
            case PlayerStages.walk:
				_moveInput = _gameInputs.Player1.Move.ReadValue<Vector2>();
				break;
            case PlayerStages.shoot:
				if (ShootTimer == 0)
				{ ShootTimer = Time.time; _moveInput = Vector2.zero; }

				if (ShootTimer + ShootTimeout < Time.time)
				{
					ShootTimer = 0;
					playerStages = PlayerStages.walk;
				}
				break;
            case PlayerStages.gotHit:
                if (HitTimer == 0) 
                { HitTimer = Time.time; _moveInput = Vector2.zero; }

                if (HitTimer + HitTimeout < Time.time) 
                {
                    HitTimer = 0;
                    playerStages = PlayerStages.walk;
                }
				break;
            case PlayerStages.died:
				if (PoofTimer == 0)
				{ 
                    PoofTimer = Time.time; 
                    _moveInput = Vector2.zero;
					audioSource.Play();
				}
                float process = (Time.time - PoofTimer) /PoofDuration;

                if (process > 0.66)
                {
					SpriteRenderer.sprite = PoofSprites[2];
				}
                else if (process > 0.33)
				{
					SpriteRenderer.sprite = PoofSprites[1];
				}
				else 
				{
					SpriteRenderer.sprite = PoofSprites[0];
				}



				if (PoofTimer + PoofDuration < Time.time)
				{
                    Debug.Log("GAMEOVER");
					Destroy(gameObject);
				}
				break;
        }
		_spriteUpdate();
	} 
    void FixedUpdate() {
        _CalcMoveSpeed();
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
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Virus" || collision.gameObject.tag == "Dino")
		{
            Health = Health - 1;
            playerStages = PlayerStages.gotHit;
            if (Health <= 0 )
            {              
                playerStages = PlayerStages.died;
            }
		}
	}
	private void _spriteUpdate()
	{
        switch (playerStages)
        {
            case PlayerStages.walk:
				if (_spriteTimer == 0)
				{
					_spriteTimer = Time.time;
				}

				if (_spriteState == 0)
				{
					SpriteRenderer.sprite = sprites[0];
				}
				else
				{
					SpriteRenderer.sprite = sprites[1];
				}

				if (_spriteTimer + spriteChangingTime < Time.time)
				{
                    _spriteTimer = 0;
                    if (_spriteState == 0) { _spriteState = 1; }
                    else { _spriteState = 0; }
				}
				break;
            case PlayerStages.gotHit:
				SpriteRenderer.sprite = sprites[3];
				break;
			case PlayerStages.shoot:
				SpriteRenderer.sprite = sprites[2];
				break;
		}

		if (_moveInput.x > 0) // find Better solution for collider flipping
		{
			SpriteRenderer.flipX = false;
			CircleCollider2D.offset = new Vector2(CircleCollider2D.offset.x * -1, CircleCollider2D.offset.y);
		}
		else if (_moveInput.x < 0)
		{
			SpriteRenderer.flipX = true;
			CircleCollider2D.offset = new Vector2(CircleCollider2D.offset.x * -1, CircleCollider2D.offset.y);
		}
	}
}
