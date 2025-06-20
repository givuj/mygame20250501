using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    MeleeFighter meleeFight;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        meleeFight = GetComponent<MeleeFighter>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()//主角攻击的主入口,和反击
    {
        if (Input.GetButtonDown("Attack"))
        {
            var enemy = EnemyManager.i.GetAttackingEnemy();//通过EnemyManager来获得EnemyController
            if (enemy != null && enemy.Fighter.IsCounterable && !meleeFight.inAction)//当反击准备好并且玩家没在攻击时,玩家可以反击
            {
                Debug.Log("反击启动");
                StartCoroutine(meleeFight.PerformCounterAttack(enemy));
            }
            else
            {
                meleeFight.TryToAttack();
            }
        }
    }
    private void OnAnimatorMove()//手动启动根运动，目的让玩家反击时不改变位置
    {
        if (!meleeFight.inCounter)//不是反击动作，位置可以随动画的移动
        {
            transform.position += animator.deltaPosition;
        }
        transform.rotation *= animator.deltaRotation;
    }
}
