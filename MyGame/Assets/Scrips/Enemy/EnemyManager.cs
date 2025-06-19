using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Vector2 timeNum = new Vector2(1,4);
    public static EnemyManager i { get; private set; }//设为公共别的地方也能用到i
    private void Awake()
    {
        i = this;
    }
    // Start is called before the first frame update
    public List<EnemyController> enemiesInRange = new List<EnemyController>();
    float notAttackingTimer = 2;   //若将其修改为 float notAttackingTimer = 0;，当没有敌人正在攻击时，由于 notAttackingTimer 初始值为 0，在 Update 方法里，if(notAttackingTimer<=0) 这个条件会立即满足，这会导致敌人会马上选择一个敌人发起攻击，而不会有任何等待时间。并且之后每次攻击结束后，虽然 notAttackingTimer 仍会被重置为 timeNum 所定义的随机值，但下次没有敌人攻击时又会立刻发起新的攻击。
    public void AddEnemyInRange(EnemyController enemy)
    {
        if(!enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }
    public void RemoveEnemyInRange(EnemyController enemy)
    {
          enemiesInRange.Remove(enemy);
        
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
            if(notAttackingTimer<=0)//满足条件敌人开始攻击
            {
                var attackingEnemy = SelectEnemyForAttack();//随机选取一个敌人
                attackingEnemy.ChangeState(EnemyStates.Attack);
                notAttackingTimer = Random.Range(timeNum.x, timeNum.y);
            }
        }

    }
    EnemyController SelectEnemyForAttack()
    {
        return enemiesInRange.OrderByDescending(e=>e.CombatMovementTimer).FirstOrDefault();
    }
}
