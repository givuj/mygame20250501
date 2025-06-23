using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;



public enum AICombatState {Idle,Chase,Circling }
public class CombatMovementState : State<EnemyController>
{
    // Start is called before the first frame update
    EnemyController enemy;
    [SerializeField] float circlingSpeed = 20f;
    [SerializeField] float  distanceToStand = 3f;
    [SerializeField] Vector2 idleTimeRange = new Vector2(2, 5);
    [SerializeField] Vector2 circlingTimeRange = new Vector2(3, 6);
    [SerializeField] float  adjust = 1f;//调整的距离
    AICombatState state;
    float timer = 0f;
    int circlingDir = 1;
    public override void Enter(EnemyController owner)
    {
        enemy = owner;
        enemy.NavAgent.stoppingDistance = distanceToStand;
        enemy.CombatMovementTimer = 0f;
       // enemy.Animator.SetBool("CombatMode", false);

    }
    public override void Execute()
    {

        if (Vector3.Distance(enemy.transform.position, enemy.Target.transform.position) > distanceToStand + adjust)
        {
           
            StartChase();
        }
        if (state == AICombatState.Idle)
        {
            if (timer <= 0)
            {
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    StartIdle();
                }
                else
                {
                    StartCircling();
                }
            }
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
            if(timer<=0)
            {
                StartIdle();
                return;
            }
            var vecToTarget = enemy.transform.position - enemy.Target.transform.position;
            var rotatedPos = Quaternion.Euler(0,circlingSpeed*circlingDir*Time.deltaTime,0)*vecToTarget;
            enemy.NavAgent.Move(rotatedPos-vecToTarget);
            enemy.transform.rotation = Quaternion.LookRotation(-rotatedPos);
        }
        if(timer>0)
        {
            timer -= Time.deltaTime;//计数器减去每帧经过的时间
        }
        enemy.CombatMovementTimer += Time.deltaTime;


    }
    void StartChase()
    {

        enemy.Animator.SetBool("CombatMode", false);
        state = AICombatState.Chase;
       
    }
    void StartIdle()
    {
        state = AICombatState.Idle;
        timer = UnityEngine.Random.Range(idleTimeRange.x,idleTimeRange.y);
        enemy.Animator.SetBool("CombatMode", true);


    }
    void StartCircling()
    {

        state = AICombatState.Circling;
        timer = Random.Range(circlingTimeRange.x, circlingTimeRange.y);
        circlingDir = Random.Range(0, 2) == 0 ? 1 : -1;
     

    }
    public override void Exit()
    {
        enemy.CombatMovementTimer = 0f;
    }
}
