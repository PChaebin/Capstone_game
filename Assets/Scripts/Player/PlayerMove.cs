using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;

    public float speed = 7.0f; // public으로 변경하여 인스펙터에서 수정 가능하게 함
    float hAxis;
    float vAxis;
    Vector3 moveVec;

    public Transform cameraTransform; // 카메라 Transform을 받아옴

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        moveVec = forward * vAxis + right * hAxis;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVec * speed * Time.fixedDeltaTime);
    }
}
