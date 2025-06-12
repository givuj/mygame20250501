using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyStates { Idle, CombatMovement }
public class EnemyController : MonoBehaviour
{
    public List<MeleeFighter> TargetsInRange { get; set; } = new List<MeleeFighter>();//��������
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
        stateMachine = new StateMachine<EnemyController>(this);//ע��û�л�ȡStateMachine<EnemyController>�Ķ���
                                                               //��ȡ���˷���״̬���У�
                                                               //����ͨ��VisionSensor��ȡ��������Ϣ
        stateMachine.ChangeState(stateDict[EnemyStates.Idle]);//ͨ��״̬�����ĸı����״̬ΪIdle״̬
        Animator = GetComponent<Animator>();



    }
    //StateMachine<T>.ChangeState������״̬�л��ĺ����߼����˳���״̬��������״̬����
    //EnemyController.ChangeState���ṩ�����õĽӿڣ���װ״̬���ҺͶ����߼���
    public void ChangeState(EnemyStates states)//EnemyStatesΪö��ͨ��Dictionary
                                               //ȷ������stateDict[states]����ΪState<EnemyController>
                                               //��IdelState�̳������ĸ���EnemyController��
    {

        stateMachine.ChangeState(stateDict[states]);
    }
    private void Update()//����Ŀ�ʼ
    {
        //var currentState = stateMachine.CurrenState;
        //if (currentState is CombatMovementState)
        //{
        //    Debug.Log("��ǰ״̬��ChaseState");
        //}
        stateMachine.Execute();
        Animator.SetFloat("moveAmount", NavAgent.velocity.magnitude / NavAgent.speed);
    }
}
