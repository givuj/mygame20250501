using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;//物体
    [SerializeField] float distant = 5;//照相机距离物体的位置
    [SerializeField] float minVerticalAngle = -45;//上下旋转度最高不超45度
    [SerializeField] float maxVerticalAngle = 45;//上下旋转度最高不超过45度
    [SerializeField] private float mouseSensitivity = 2f;//鼠标灵敏度
    float rotationX;
    float rotationY;
    [SerializeField] Vector2 framingOffset;//最开始照相机对于物体的偏移量
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
        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);//照相机旋转多少度
        var focusPostion = followTarget.position + new Vector3(framingOffset.x, framingOffset.y,0);
        transform.position = focusPostion -targetRotation * new Vector3(0, 0, distant);//直接设置游戏对象(照相机)的位置
        transform.rotation = targetRotation;//直接设置游戏对象(照相机)的旋转角度

    }
    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);
}
