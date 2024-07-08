using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;

    public float speed = 7.0f; // public���� �����Ͽ� �ν����Ϳ��� ���� �����ϰ� ��
    float hAxis;
    float vAxis;
    Vector3 moveVec;

    public Transform cameraTransform; // ī�޶� Transform�� �޾ƿ�

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
