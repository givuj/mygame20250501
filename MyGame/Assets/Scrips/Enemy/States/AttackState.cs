using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<EnemyController>
{
    // Start is called before the first frame update
    [SerializeField] float attackDistance = 1f;
    bool isAttacking;
    EnemyController enemy;
    public override void Enter(EnemyController owner)
    {
        enemy = owner;
        enemy.NavAgent.stoppingDistance = attackDistance;
     
    }
    public override void Execute()
    {
        if (isAttacking) return;
        if(enemy.Target==null)
        {
            return;
        }
        enemy.NavAgent.SetDestination(enemy.Target.transform.position);
        if (Vector3.Distance(enemy.transform.position, enemy.Target.transform.position) <= attackDistance + 0.03f)
        {
            
            StartCoroutine(Attack(Random.Range(1,6)));
        }
           
    }
    IEnumerator Attack(int combCount=1)//敌人攻击，包含连击
    {
        isAttacking = true;//因为update是每帧都调用
        enemy.Animator.applyRootMotion = true;
        enemy.Fighter.TryToAttack();
        for(int i=1;i<combCount ;i++)//连击此时，当combCount为3时，实施2次连击，相当于两次把doComb变为true可以执行两次
                                     //MeleeFighter中if(doComb)这个语句
      
        {
            yield return new WaitUntil(() => enemy.Fighter.attackStates == AttackStates.Cooldown);//这个不是等待语句这个是检测语句
                                                                                                  //等待是yield return new WaitForSeconds 这个        
            enemy.Fighter.TryToAttack();
        }
        yield return new WaitUntil(() => enemy.Fighter.attackStates == AttackStates.Idle);//可以去看MeleeFighter中的TryToAttack
        enemy.Animator.applyRootMotion = false;                                                                                  //当attackStates = AttackStates.Idle;就代表结束了一次攻击
        isAttacking = false;
        if(enemy.IsInState(EnemyStates.Attack))//当敌人处于死亡状态时就不能执行后撤的动作
        enemy.ChangeState(EnemyStates.RetreatAfterAttack);
    }
    public override void Exit()
    {
        enemy.NavAgent.ResetPath();
    }
}
