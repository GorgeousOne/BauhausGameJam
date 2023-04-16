using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowDinoAI : DinoAI
{
    [Header("Movementpattern 1 Settings")]
    protected Vector2 TargetVector;
    float timer2;
    protected int movementStage;
    public float targetVectorUpdatetime = 1;


    // Start is called before the first frame update

    // Update is called once per frame


    protected override void MovementStage1()
    {
        switch(movementStage) 
        {
            case 0: // Targeting stage
                TargetVector = Target.transform.position - transform.position;
                movementStage = 1;
                timer2 = Time.time;

                break;
            case 1:
                _moveInput = TargetVector.normalized;
                
                if(timer2+targetVectorUpdatetime < Time.time){
                    movementStage = 0;
                }
                break;
        }

    }
}
