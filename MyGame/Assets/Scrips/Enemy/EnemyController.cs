using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyStates { Idle, CombatMovement }
public class EnemyController : MonoBehaviour
{
    public List<MeleeFighter> TargetsInRange { get; set; } = new List<MeleeFighter>();//主角数量
    [field: SerializeField] public float Fov { get; private set; } = 180f;//
    public MeleeFighter Target { get; set; }
    public NavMeshAgent NavAgent { get; private set; }
    public StateMachine<EnemyController> stateMachine { get; private set; }
    public Animator Animator { get; private set; }
    Dictionary<EnemyStates, State<EnemyController>> stateDict;
    private void Start()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        stateDict = new Dictionary<EnemyStates, State<EnemyController>>();
        stateDict[EnemyStates.Idle] = GetComponent<IdleState>();
        stateDict[EnemyStates.CombatMovement] = GetComponent<CombatMovementState>();
        stateMachine = new StateMachine<EnemyController>(this);//注意没有获取StateMachine<EnemyController>的对象
                                                               //获取敌人放入状态机中，
                                                               //敌人通过VisionSensor获取了主角信息
        stateMachine.ChangeState(stateDict[EnemyStates.Idle]);//通过状态机核心改变敌人状态为Idle状态
        Animator = GetComponent<Animator>();



    }
    //StateMachine<T>.ChangeState：负责状态切换的核心逻辑（退出旧状态、进入新状态）。
    //EnemyController.ChangeState：提供更易用的接口，封装状态查找和额外逻辑。
    public void ChangeState(EnemyStates states)//EnemyStates为枚举通过Dictionary
                                               //确定类型stateDict[states]类型为State<EnemyController>
                                               //（IdelState继承了他的父类EnemyController）
    {

        stateMachine.ChangeState(stateDict[states]);
    }
    Vector3 prevPos;
    private void Update()//任务的开始
    {
        stateMachine.Execute();
        var deltaPos = transform.position - prevPos;
        var velocity = deltaPos / Time.deltaTime;
        float forwardSpeed = Vector3.Dot(velocity,transform.forward);
        Animator.SetFloat("moveAmount", forwardSpeed / NavAgent.speed,0.2f,Time.deltaTime);
        float angle = Vector3.SignedAngle(transform.forward,velocity,Vector3.up);
        float strafeSpeed = Mathf.Sin(angle*Mathf.Deg2Rad);
        Animator.SetFloat("strafeSpeed", strafeSpeed,0.2f,Time.deltaTime);
        prevPos = transform.position;  
    }
}
