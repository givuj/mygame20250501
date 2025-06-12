using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//攻击动画
public enum AttackState {Idle,Windup,Inpact,Cooldown };//攻击的状态，只有使出挥剑的动作才能有被攻击的状态
public class MeleeFighter : MonoBehaviour
{
    int doCombCount = 0;
    [SerializeField] List<AttackData> attacks;
    [SerializeField] GameObject sword;//看要哪个物体的碰撞器，把那个物体名字在检测器上写上去
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
        //这个else if很牛逼，因为update能改变协程中的docomb的值，同样的协程中也能改变updata的值，这个TryToAttack()是外部update调用
        //当按下第一次攻击时，进入if，然后进入协程 inAction变为true，在第一次动画中再次按下攻击，因为协程还没有结束，
        //inAction一直是true，不进入if，但是协程中攻击状态（attackState）已经改变所有进入else if docomb变成true协程中触发连击
        //总的来说就是相当于只有在攻击的后半段在按攻击才能进入连击
        else if (attackState == AttackState.Cooldown|| attackState == AttackState.Inpact)
        {
            doComb = true;
        }
    }
    //攻击的进阶写法有扣血和被攻击到的人的反应的动画+连击（这个连击很精妙）
    IEnumerator Attack()//协程
    {

        Debug.Log(doCombCount);
        inAction = true;// 标记攻击开始
        attackState = AttackState.Windup;
        //float impactStartTime = 0.33f;//选择攻击动画中造成伤害的时间点（就是挥出去那一段造成伤害时间段记下来）
        //float impactEndTime = 0.55f;

        animator.CrossFade(attacks[doCombCount].AnimName, 0.2f);// 播放攻击动画
        yield return null;


        var animState = animator.GetNextAnimatorStateInfo(1);// 获取当前动画状态
        float timer = 0f;//计时
        while (timer<= animState.length) //这个循环的总体效果和yield return new WaitForSeconds(animState.length);
                                         //是一样的，只是这个循环可以在等待动画结束的过程加入东西，比如判断这段伤害有没有打中敌人啊等等
        {
            timer += Time.deltaTime;//计算时间
            float normalizedTime = timer / animState.length;//统一化
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
                    yield break;//做完动作后退出循环
                }
            }
            

            yield return null;//等待一帧在这个循环中就是等待直到这个动画结束
        }
        attackState = AttackState.Idle;
        doCombCount = 0;
        inAction = false;

    }



    //攻击一般写法，不包含被攻击的人的扣血动画的东西，只有主动攻击的动画
    //IEnumerator Attack()//协程
    //{
    //    inAction = true;// 标记攻击开始
    //    animator.CrossFade("Slash", 0.2f);// 播放攻击动画
    //    yield return null;// 等待一帧，确保动画状态更新****（    当前帧：仅记录动画过渡请求（状态机尚未切换）下一帧：动画系统才会真正完成状态切换，更新为新动画）


    //    var animState = animator.GetNextAnimatorStateInfo(1);// 获取当前动画状态
    //    yield return new WaitForSeconds(animState.length);// 等待动画播放完毕
    //    inAction = false;

    //}

    //敌人被剑击中动画和逻辑
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
