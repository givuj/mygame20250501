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
            StartCoroutine(Attack());
        }
           
    }
    IEnumerator Attack()
    {
        isAttacking = true;
        enemy.Animator.applyRootMotion = true;
        enemy.Fighter.TryToAttack();
        yield return new WaitUntil(() => enemy.Fighter.attackStates == AttackStates.Idle);//可以去看MeleeFighter中的TryToAttack
        enemy.Animator.applyRootMotion = false;                                                                                  //当attackStates = AttackStates.Idle;就代表结束了一次攻击
        isAttacking = false;
    }
}
