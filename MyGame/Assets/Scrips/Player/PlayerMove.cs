using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMove : MonoBehaviour
{
    [SerializeField] float movespeed = 5f;
    [SerializeField] float rotationSpeed = 500f;
    Quaternion targetRotation;
    CameraController cameraController;//摄影机主体
    CharacterController characterController;//碰撞主体也可以说就是物体世界的主体
    Animator animator;//动画主体
    //检测物体是否在空中的主体
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;
    bool isGrounded;
    //y方向的重力
    float ySpeed;
    //战斗动作
    MeleeFighter meleeFighter;
    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        meleeFighter = GetComponent<MeleeFighter>();
    }

   
    void Update()
    {
       
        if (meleeFighter.inAction)//确保我们按下攻击键不会移动
        {
            animator.SetFloat("moveAmount", 0f);//确保攻击停止时不会是跑或者走的状态   
            return; 
        }
        // 使用原始输入，立即获取0值
        float h = Input.GetAxisRaw("Horizontal");//当不输入wsad是h会以及减成0，这样可以防止停止移动时的滑行
        float v = Input.GetAxisRaw("Vertical");//当不输入wsad是v会以及减成0

        // 计算移动量，确保输入为0时moveAmount为0
        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));
        var moveInput = new Vector3(h, 0, v).normalized;
        var moveDir = cameraController.PlanarRotation * moveInput;
        GroundCheck();

        if (isGrounded)//看是否在地面上还是在空中,来判断是否增加重力
        {
            ySpeed = -0.5f;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }
        var velocity = moveDir * movespeed;
        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);//有重力有碰撞人物移动的距离
        if (moveAmount > 0)//当视角移动量大于0时人物才能移动
        {
            targetRotation = Quaternion.LookRotation(moveDir);
            //transform.position += moveDir * movespeed * Time.deltaTime;//没物理世界人物移动的距离
           
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);//人物移动的方向
        animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);//播放走和跑的动画，并且是逐步的播放
    }
    
    //检测是否与地面接触，这个物体没有显示出来
    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset),groundCheckRadius,groundLayer);

    }
    //将这个小物体显示出来，就是脚下的绿色的小圆，这个小圆没有任何作用只为让你看的更清楚
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset),groundCheckRadius);
    }
}   
