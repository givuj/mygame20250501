using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��������
public enum AttackState {Idle,Windup,Inpact,Cooldown };//������״̬��ֻ��ʹ���ӽ��Ķ��������б�������״̬
public class MeleeFighter : MonoBehaviour
{
    int doCombCount = 0;
    [SerializeField] List<AttackData> attacks;
    [SerializeField] GameObject sword;//��Ҫ�ĸ��������ײ�������Ǹ����������ڼ������д��ȥ
    BoxCollider swordCollider;
    SphereCollider leftHandCollide, rightFootCollide;
    Animator animator;
    public void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        if(sword!=null)
        {
            swordCollider = sword.GetComponent<BoxCollider>();
            rightFootCollide = animator.GetBoneTransform(HumanBodyBones.RightFoot).GetComponent<SphereCollider>();
            swordCollider.enabled = false;
            rightFootCollide.enabled = false;
        }
    }
    public AttackState attackState; 
    public bool inAction { get; private set; } = false;
    bool doComb;
    
    public void TryToAttack()
    {
        if(!inAction)
        {
           StartCoroutine(Attack());
          
        }
        //���else if��ţ�ƣ���Ϊupdate�ܸı�Э���е�docomb��ֵ��ͬ����Э����Ҳ�ܸı�updata��ֵ�����TryToAttack()���ⲿupdate����
        //�����µ�һ�ι���ʱ������if��Ȼ�����Э�� inAction��Ϊtrue���ڵ�һ�ζ������ٴΰ��¹�������ΪЭ�̻�û�н�����
        //inActionһֱ��true��������if������Э���й���״̬��attackState���Ѿ��ı����н���else if docomb���trueЭ���д�������
        //�ܵ���˵�����൱��ֻ���ڹ����ĺ����ڰ��������ܽ�������
        else if (attackState == AttackState.Cooldown|| attackState == AttackState.Inpact)
        {
            doComb = true;
        }
    }
    //�����Ľ���д���п�Ѫ�ͱ����������˵ķ�Ӧ�Ķ���+��������������ܾ��
    IEnumerator Attack()//Э��
    {

        Debug.Log(doCombCount);
        inAction = true;// ��ǹ�����ʼ
        attackState = AttackState.Windup;
        //float impactStartTime = 0.33f;//ѡ�񹥻�����������˺���ʱ��㣨���ǻӳ�ȥ��һ������˺�ʱ��μ�������
        //float impactEndTime = 0.55f;

        animator.CrossFade(attacks[doCombCount].AnimName, 0.2f);// ���Ź�������
        yield return null;


        var animState = animator.GetNextAnimatorStateInfo(1);// ��ȡ��ǰ����״̬
        float timer = 0f;//��ʱ
        while (timer<= animState.length) //���ѭ��������Ч����yield return new WaitForSeconds(animState.length);
                                         //��һ���ģ�ֻ�����ѭ�������ڵȴ����������Ĺ��̼��붫���������ж�����˺���û�д��е��˰��ȵ�
        {
            timer += Time.deltaTime;//����ʱ��
            float normalizedTime = timer / animState.length;//ͳһ��
            if (attackState == AttackState.Windup)
            {
                if (normalizedTime >= attacks[doCombCount].ImpactStartime)
                {
                    attackState = AttackState.Inpact;
                    swordCollider.enabled = true;
                    rightFootCollide.enabled = true;
                }
                
            }
            else if (attackState == AttackState.Inpact)
            {
                if (normalizedTime >= attacks[doCombCount].ImpactEndtime)
                {
                    attackState = AttackState.Cooldown;
                    swordCollider.enabled = false;
                    rightFootCollide.enabled = false;
                }
            }
            else if (attackState == AttackState.Cooldown)
            {
                if(doComb)
                {
                    doComb = false;
                    doCombCount = (doCombCount + 1) % attacks.Count;
                    StartCoroutine(Attack()); 
                    yield break;//���궯�����˳�ѭ��
                }
            }
            

            yield return null;//�ȴ�һ֡�����ѭ���о��ǵȴ�ֱ�������������
        }
        attackState = AttackState.Idle;
        doCombCount = 0;
        inAction = false;

    }



    //����һ��д�������������������˵Ŀ�Ѫ�����Ķ�����ֻ�����������Ķ���
    //IEnumerator Attack()//Э��
    //{
    //    inAction = true;// ��ǹ�����ʼ
    //    animator.CrossFade("Slash", 0.2f);// ���Ź�������
    //    yield return null;// �ȴ�һ֡��ȷ������״̬����****��    ��ǰ֡������¼������������״̬����δ�л�����һ֡������ϵͳ�Ż��������״̬�л�������Ϊ�¶�����


    //    var animState = animator.GetNextAnimatorStateInfo(1);// ��ȡ��ǰ����״̬
    //    yield return new WaitForSeconds(animState.length);// �ȴ������������
    //    inAction = false;

    //}

    //���˱������ж������߼�
    private void OnTriggerEnter(Collider other)
    {
      
        if (other.tag=="Hitbox"&&!inAction)
        {
            StartCoroutine(PlayHitReaction());
        }
    }
    IEnumerator PlayHitReaction()
    {
        inAction = true;
        animator.CrossFade("SwordImpact", 0.2f);
        yield return null;


        var animState = animator.GetNextAnimatorStateInfo(1);
        yield return new WaitForSeconds(animState.length*0.2f);
        inAction = false;

    }
}
