using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;//����
    [SerializeField] float distant = 5;//��������������λ��
    [SerializeField] float minVerticalAngle = -45;//������ת����߲���45��
    [SerializeField] float maxVerticalAngle = 45;//������ת����߲�����45��
    [SerializeField] private float mouseSensitivity = 2f;//���������
    float rotationX;
    float rotationY;
    [SerializeField] Vector2 framingOffset;//�ʼ��������������ƫ����
    // Start is called before the first frame update
    void SetCursorState()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start()
    {
        SetCursorState();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        rotationX = Mathf.Clamp(rotationX + Input.GetAxis("Mouse Y") * mouseSensitivity, minVerticalAngle, maxVerticalAngle);
        rotationY += Input.GetAxis("Mouse X")* mouseSensitivity;
        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);//�������ת���ٶ�
        var focusPostion = followTarget.position + new Vector3(framingOffset.x, framingOffset.y,0);
        transform.position = focusPostion -targetRotation * new Vector3(0, 0, distant);//ֱ��������Ϸ����(�����)��λ��
        transform.rotation = targetRotation;//ֱ��������Ϸ����(�����)����ת�Ƕ�

    }
    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);
}
