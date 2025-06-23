using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public EnemyController targetEnemy;//锁定视角的最近的敌人
    MeleeFighter meleeFight;
    Animator animator;
    CameraController cam;

    public bool IsPutMouse2 { get; set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        meleeFight = GetComponent<MeleeFighter>();
        animator = GetComponent<Animator>();
        cam = Camera.main.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()//主角攻击的主入口,和反击
    {
        if (Input.GetButtonDown("Attack"))
        {
            var enemy = EnemyManager.i.GetAttackingEnemy();//通过EnemyManager来获得EnemyController
            if (enemy != null && enemy.Fighter.IsCounterable && !meleeFight.inAction)//当反击准备好并且玩家没在攻击时,玩家可以反击
            {

                StartCoroutine(meleeFight.PerformCounterAttack(enemy));
            }
            else
            {
                meleeFight.TryToAttack();
            }
        }
        if (Input.GetButtonDown("LockOn"))//锁定敌人视角
        {
            IsPutMouse2 = (!IsPutMouse2);//最开始变成ture
            if (!IsPutMouse2)
            {
                targetEnemy?.MeshHighlighter?.HighlightMesh(false);
                targetEnemy = null;
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
    public Vector3 GetTargetingDir()//获得相机和人物之间的向量反向，以便找离得最近的敌人锁定视角
    {

        var vecFromCam = transform.position - cam.transform.position;
        vecFromCam.y = 0f;
        return vecFromCam.normalized;
    }
}
