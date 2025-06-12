using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMove : MonoBehaviour
{
    [SerializeField] float movespeed = 5f;
    [SerializeField] float rotationSpeed = 500f;
    Quaternion targetRotation;
    CameraController cameraController;//��Ӱ������
    CharacterController characterController;//��ײ����Ҳ����˵�����������������
    Animator animator;//��������
    //��������Ƿ��ڿ��е�����
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;
    bool isGrounded;
    //y���������
    float ySpeed;
    //ս������
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
       
        if (meleeFighter.inAction)//ȷ�����ǰ��¹����������ƶ�
        {
            animator.SetFloat("moveAmount", 0f);//ȷ������ֹͣʱ�������ܻ����ߵ�״̬   
            return; 
        }
        // ʹ��ԭʼ���룬������ȡ0ֵ
        float h = Input.GetAxisRaw("Horizontal");//��������wsad��h���Լ�����0���������Է�ֹֹͣ�ƶ�ʱ�Ļ���
        float v = Input.GetAxisRaw("Vertical");//��������wsad��v���Լ�����0

        // �����ƶ�����ȷ������Ϊ0ʱmoveAmountΪ0
        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));
        var moveInput = new Vector3(h, 0, v).normalized;
        var moveDir = cameraController.PlanarRotation * moveInput;
        GroundCheck();

        if (isGrounded)//���Ƿ��ڵ����ϻ����ڿ���,���ж��Ƿ���������
        {
            ySpeed = -0.5f;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }
        var velocity = moveDir * movespeed;
        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);//����������ײ�����ƶ��ľ���
        if (moveAmount > 0)//���ӽ��ƶ�������0ʱ��������ƶ�
        {
            targetRotation = Quaternion.LookRotation(moveDir);
            //transform.position += moveDir * movespeed * Time.deltaTime;//û�������������ƶ��ľ���
           
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);//�����ƶ��ķ���
        animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);//�����ߺ��ܵĶ������������𲽵Ĳ���
    }
    
    //����Ƿ������Ӵ����������û����ʾ����
    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset),groundCheckRadius,groundLayer);

    }
    //�����С������ʾ���������ǽ��µ���ɫ��СԲ�����СԲû���κ�����ֻΪ���㿴�ĸ����
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset),groundCheckRadius);
    }
}   
