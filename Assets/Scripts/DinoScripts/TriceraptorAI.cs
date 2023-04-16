using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriceraptorAI : DinoAI
{
	// can charge at you in straight line fast, than he rests for 2 sec
	[Header("Movementpattern 1 Settings")]
	[SerializeField] private float prepTime = 3;
	[SerializeField] private float moveTime = 3;
	[SerializeField] private float wiggleDist = .5f;

	private bool _isRunningUp = true;

	private float _prepStart;
	private Vector2 _prepPos;

	private float _moveStart;
	private Vector2 _moveDir;
	
	new void Start()
	{
		base.Start();
		_moveDir = getTargetDist().normalized;
		_prepPos = transform.position;
	}

	protected override void MovementStage1()
	{
		if (_isRunningUp)
		{

			if (Time.time > _moveStart + moveTime)
			{
				_isRunningUp = false;
				_prepStart = Time.time;
				_prepPos = transform.position;
				_moveDir = getTargetDist().normalized;
			}
		}
		else {
			float dTime = Time.time - _prepStart;
			transform.position = _prepPos + Mathf.Sin(4 * Mathf.PI * dTime) * wiggleDist * _moveDir;

			if (Time.time > _prepStart + prepTime)
			{
				_isRunningUp = true;
				_moveStart = Time.time;
				_rigid.velocity = speed * _moveDir;
			}
		}
	}
}
