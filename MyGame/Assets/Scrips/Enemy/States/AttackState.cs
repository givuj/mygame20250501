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
            Debug.Log("尝试启动攻击协程");
            StartCoroutine(Attack(Random.Range(0,4)));
        }
           
    }
    IEnumerator Attack(int combCount=1)//敌人攻击，包含连击
    {
        isAttacking = true;//因为update是每帧都调用
        enemy.Animator.applyRootMotion = true;
        enemy.Fighter.TryToAttack();
        for(int i=1;i<combCount ;i++)
        {
            yield return new WaitUntil(() => enemy.Fighter.attackStates == AttackStates.Cooldown);//这个不是等待语句这个是检测语句，只有满足条件才会执行下一个
                                                                                                  //等待是yield return new WaitForSeconds 这个        
            enemy.Fighter.TryToAttack();
        }
        yield return new WaitUntil(() => enemy.Fighter.attackStates == AttackStates.Idle);//可以去看MeleeFighter中的TryToAttack
        enemy.Animator.applyRootMotion = false;                                                                                  //当attackStates = AttackStates.Idle;就代表结束了一次攻击
        isAttacking = false;
        enemy.ChangeState(EnemyStates.RetreatAfterAttack);
    }
    public override void Exit()
    {
        enemy.NavAgent.ResetPath();
    }
}
