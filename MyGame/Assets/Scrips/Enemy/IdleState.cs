using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State<EnemyController>
{
    EnemyController enemy;
    // Start is called before the first frame update
    public override void Enter(EnemyController owner)
    {
        enemy = owner;
        
    }
    public override void Execute()
    {
      
       foreach(var target in enemy.TargetsInRange)//在视线内就切换为追逐形态
       {
          
            var verToTarget = target.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward,verToTarget);
            if(angle<=enemy.Fov/2)
            {
                
                enemy.Target = target;
                enemy.ChangeState(EnemyStates.CombatMovement);
                break;
            }
       }
    }
    public override void Exit()
    {
        Debug.Log("Exit");
    }
}
