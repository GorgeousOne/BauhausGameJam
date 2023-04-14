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
    
    void Awake() {
        _rigid = GetComponent<Rigidbody2D>();
        _gameInputs = new GameInputs();
        _gameInputs.Player1.Enable();
    }

    private Vector2 _moveInput;
    
    void Update() {
        _moveInput = _gameInputs.Player1.Move.ReadValue<Vector2>();
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
}
