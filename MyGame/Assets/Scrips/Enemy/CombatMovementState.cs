using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AICombatState {Idle,Chase,Circling }
public class CombatMovementState : State<EnemyController>
{
    // Start is called before the first frame update
    EnemyController enemy;
    [SerializeField] float  distanceToStand = 3f;
    [SerializeField] float  adjust = 1f;//µ÷ÕûµÄ¾àÀë
    AICombatState state;
    public override void Enter(EnemyController owner)
    {
        enemy = owner;
        enemy.NavAgent.stoppingDistance = distanceToStand;
       
    }
    public override void Execute()
    {

        if (Vector3.Distance(enemy.transform.position, enemy.Target.transform.position) > distanceToStand + adjust)
        {
            Debug.Log(4);
            StartChase();
        }
        if (state == AICombatState.Idle)
        {


        }
        else if(state == AICombatState.Chase )
        {
            
            if (Vector3.Distance(enemy.Target.transform.position, enemy.transform.position)<=distanceToStand+0.03)
            {
          
                StartIdle();
                return;
            }
           
            enemy.NavAgent.SetDestination(enemy.Target.transform.position);
        }
        else if(state == AICombatState.Circling)
        {

        }
        
        
    }
    void StartChase()
    {
        Debug.Log(5);
        state = AICombatState.Chase;
        enemy.Animator.SetBool("CombatMode",false);
    }
    void StartIdle()
    {
        state = AICombatState.Idle;
        enemy.Animator.SetBool("CombatMode", true);
    }

    public override void Exit()
    {
        Debug.Log("Exit chase");
    }
}
