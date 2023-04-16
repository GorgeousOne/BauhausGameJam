using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamosaurusAI : DinoAI
{
	// spins in circular pattern, middle speed

	
	protected Vector2 TargetVector;
	float timer2;
	protected int movementStage;
	[Header("Movementpattern 1 Settings")]
	public float targetVectorUpdatetime = 1;
	public float circlesize = 1;
	public float circlespeed = 1;





	protected override void MovementStage1()
	{
		switch (movementStage)
		{
			case 0: // Targeting stage
				TargetVector = Target.transform.position - transform.position;
				movementStage = 1;
				timer2 = Time.time;

				break;
			case 1:
				_moveInput = TargetVector.normalized + new Vector2(Mathf.Cos(Time.time * circlespeed), Mathf.Sin(Time.time) * circlespeed) * circlesize;

				if (timer2 + targetVectorUpdatetime < Time.time)
				{
					movementStage = 0;
				}
				break;
		}

	}
}
