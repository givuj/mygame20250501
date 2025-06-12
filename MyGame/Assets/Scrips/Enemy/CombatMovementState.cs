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
        
    }
    void StartChase()
    {
        Debug.Log(5);
        state = AICombatState.Chase;
        enemy.Animator.SetBool("CombatMode",false);
        enemy.Animator.SetBool("circling", false);
    }
    void StartIdle()
    {
        state = AICombatState.Idle;
        timer = UnityEngine.Random.Range(idleTimeRange.x,idleTimeRange.y);
        enemy.Animator.SetBool("CombatMode", true);
        enemy.Animator.SetBool("circling", false);

    }
    void StartCircling()
    {

        state = AICombatState.Circling;
        timer = Random.Range(circlingTimeRange.x, circlingTimeRange.y);
        circlingDir = Random.Range(0, 2) == 0 ? 1 : -1;
        enemy.Animator.SetBool("circling", true);    
        enemy.Animator.SetFloat("circlingDir", circlingDir);    

    }
    public override void Exit()
    {
        Debug.Log("Exit chase");
    }
}
