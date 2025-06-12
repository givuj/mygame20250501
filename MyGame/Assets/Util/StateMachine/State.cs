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
//.��ɫ�ֹ�
//��                     ����	                        �ؼ��߼�
//EnemyController	     ״̬���������ߣ�����˶���	��ʼ��״̬�����ṩ״̬�л��ӿ�
//State<T>	             ״̬�Ļ���	                    ����״̬���������ڷ�����Enter/Execute/Exit��
//IdleState/ChaseState	 ����״̬	                    ʵ��״̬�ľ�����Ϊ���� idle ʱ�����ң�chase ʱ׷��
//StateMachine<T>	     ״̬������	                    ����״̬�л���ChangeState����ִ�У�Execute��
//EnemyStates	         ״̬ö��	                    ���״̬���ͣ�Idle/Chase���������ֵ����