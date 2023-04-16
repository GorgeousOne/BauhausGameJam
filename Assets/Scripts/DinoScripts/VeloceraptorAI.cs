using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeloceraptorAI : DinoAI
{
	// can jump in zig-zag pattern, faster than others

	
	protected Vector2 TargetVector;
	float timer2;
	protected int movementStage;
	[Header("Movementpattern 1 Settings")]
	public float targetVectorUpdatetime = 1;
	public float Zigzagrate = 1;
	bool ZigUp = true;
	float ZigTimer = 0;
	Vector2 _secondDir = Vector2.zero;




	protected override void MovementStage1()
	{
		switch (movementStage)
		{
			case 0: // Targeting stage
				TargetVector = Target.transform.position - transform.position;
				TargetVector = TargetVector.normalized;
				
				movementStage = 1;
				timer2 = Time.time;

				break;
			case 1:
				switch (ZigUp)
				{
					case true:
						if (ZigTimer == 0) { ZigTimer = Time.time; }
						_secondDir = new Vector2(Mathf.Cos(Mathf.Asin(TargetVector.x) + 1.570796f), Mathf.Sin(Mathf.Asin(TargetVector.y) + 1.570796f));
						if (ZigTimer+Zigzagrate < Time.time) 
						{ 
							ZigUp = false;
							ZigTimer = 0;
						}
						break;
					case false:
						if (ZigTimer == 0) { ZigTimer = Time.time; }
						_secondDir = new Vector2(Mathf.Cos(Mathf.Asin(TargetVector.x) - 1.570796f), Mathf.Sin(Mathf.Asin(TargetVector.y) - 1.570796f));
						if (ZigTimer + Zigzagrate < Time.time)
						{
							ZigUp = true;
							ZigTimer = 0;
						}
						break;
				}

				_moveInput = (TargetVector+_secondDir).normalized;

				if (timer2 + targetVectorUpdatetime < Time.time)
				{
					movementStage = 0;
				}
				break;
		}

	}
}
