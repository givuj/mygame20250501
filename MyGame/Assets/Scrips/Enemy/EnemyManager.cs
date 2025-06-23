using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Vector2 timeNum = new Vector2(1, 4);
    [SerializeField] CombatController player;
    [SerializeField] float lockOnDistance = 3f;
    private bool isLocked = false; // 标记是否已经锁定了敌人
    public static EnemyManager i { get; private set; }//设为公共别的地方也能用到i
    private void Awake()
    {
        i = this;
    }
    // Start is called before the first frame update
    public List<EnemyController> enemiesInRange = new List<EnemyController>();
    float notAttackingTimer = 2;   //若将其修改为 float notAttackingTimer = 0;，当没有敌人正在攻击时，由于 notAttackingTimer 初始值为 0，在 Update 方法里，if(notAttackingTimer<=0) 这个条件会立即满足，这会导致敌人会马上选择一个敌人发起攻击，而不会有任何等待时间。并且之后每次攻击结束后，虽然 notAttackingTimer 仍会被重置为 timeNum 所定义的随机值，但下次没有敌人攻击时又会立刻发起新的攻击。
    private EnemyController lockedEnemy = null;//已经锁定的敌人
    public void AddEnemyInRange(EnemyController enemy)
    {
        if (!enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }
    float timer = 0f;
    public void RemoveEnemyInRange(EnemyController enemy)
    {
        enemiesInRange.Remove(enemy);
        if (enemy == player.targetEnemy)
        {
            enemy.MeshHighlighter?.HighlightMesh(false);
            if (isLocked) // 锁定模式：清空目标但不切换,player.IsPutMouse2 = !player.IsPutMouse2;关键一步
            {
                player.targetEnemy = null;
                lockedEnemy = null;
                isLocked = false;
                player.IsPutMouse2 = !player.IsPutMouse2;
            }
            else // 非锁定模式：正常切换到最近敌人
            {
                player.targetEnemy = GetClosesEnemyToPlayerDir();
                player.targetEnemy?.MeshHighlighter?.HighlightMesh(true);
            }

        }
    }
    private void Update()//敌人攻击主角的主入口，以及将敌人变为攻击状态
    {
        
        if (enemiesInRange.Count == 0) return;
        if (!enemiesInRange.Any(e => e.IsInState(EnemyStates.Attack)))//2. .Any(...)方法：Enumerable.Any 扩展方法 作用：判断集合中 是否至少有一个元素满足指定条件，返回 bool。
        {                                                          //3.e => e.IsInState(EnemyStates.Attack) 含义：对每个敌人 e，调用其 IsInState 方法，检查是否处于 EnemyStates.Attack 状态。
            if (notAttackingTimer > 0)
            {
                notAttackingTimer -= Time.deltaTime;
            }
            if (notAttackingTimer <= 0)//满足条件敌人开始攻击
            {
                var attackingEnemy = SelectEnemyForAttack();//随机选取一个敌人
                if (attackingEnemy != null)
                {
                    attackingEnemy.ChangeState(EnemyStates.Attack);
                    notAttackingTimer = Random.Range(timeNum.x, timeNum.y);
                }
            }
        }
        if (timer > 0.01f)
        {

            timer = 0f;
            if (player.IsPutMouse2)
            {
                // 如果玩家按下鼠标中键，且没有锁定敌人，则锁定最近的敌人
                if (!isLocked)
                {
                    lockedEnemy = GetClosesEnemyToPlayerDir();
                    if (lockedEnemy != null && Vector3.Distance(player.transform.position, lockedEnemy.transform.position) <= lockOnDistance)
                    {
                        player.targetEnemy = lockedEnemy;
                        lockedEnemy.MeshHighlighter.HighlightMesh(true);
                        isLocked = true;
                    }
                }
            }
            else
            {
                   // 如果玩家松开鼠标中键，取消锁定
               
                    isLocked = false;
                
            }
        }

        timer += Time.deltaTime;

    }
   

    EnemyController SelectEnemyForAttack()
    {
        return enemiesInRange.OrderByDescending(e => e.CombatMovementTimer).FirstOrDefault(e => e.Target != null);
    }
    public EnemyController GetAttackingEnemy()
    {
        return enemiesInRange.FirstOrDefault(e => e.IsInState(EnemyStates.Attack));
    }
    public EnemyController GetClosesEnemyToPlayerDir()//获取最近的敌人
    {

        var targetingDir = player.GetTargetingDir();

        float minDistance = Mathf.Infinity;
        EnemyController closestEnemy = null;
        foreach (var enemy in enemiesInRange)
        {
            var vecToEnemy = enemy.transform.position - player.transform.position;
            vecToEnemy.y = 0;
            float angle = Vector3.Angle(targetingDir, vecToEnemy);
            float distance = vecToEnemy.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad);//sin度数
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

}
