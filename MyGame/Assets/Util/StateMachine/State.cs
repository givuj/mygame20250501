using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State<T>:MonoBehaviour
{
    // Start is called before the first frame update
    public virtual void Enter(T owner) { }
    public virtual void Execute() { }
    public virtual void Exit() { }
}
//.角色分工
//类                     作用	                        关键逻辑
//EnemyController	     状态机的所有者（如敌人对象）	初始化状态机、提供状态切换接口
//State<T>	             状态的基类	                    定义状态的生命周期方法（Enter/Execute/Exit）
//IdleState/ChaseState	 具体状态	                    实现状态的具体行为（如 idle 时检测玩家，chase 时追逐）
//StateMachine<T>	     状态机核心	                    管理状态切换（ChangeState）和执行（Execute）
//EnemyStates	         状态枚举	                    标记状态类型（Idle/Chase），方便字典管理