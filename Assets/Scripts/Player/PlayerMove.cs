using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;

    public float speed = 7.0f; // �Ϲ� �ӵ�
    public float runSpeed = 10.0f; // �޸��� �ӵ�
    public float dashSpeed = 20.0f; // �뽬 �ӵ�
    public float dashDuration = 0.2f; // �뽬 ���� �ð�
    public float jumpForce = 5.0f; // ���� ��

    private bool isDashing = false;
    private bool isGrounded = true;

    float hAxis;
    float vAxis;
    Vector3 moveVec;

    bool isBorder;

    public Transform cameraTransform; // ī�޶� Transform�� �޾ƿ�

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetInput();
        Run();
        Dash();
        Jump();
        Debug.Log(moveVec);
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f; // ī�޶��� y�� ������ �����Ͽ� ���� �̵��� �ݿ�
        right.y = 0f;   // ī�޶��� y�� ������ �����Ͽ� ���� �̵��� �ݿ�

        forward.Normalize();
        right.Normalize();

        moveVec = forward * vAxis + right * hAxis;
        moveVec *= speed; // �⺻ �ӵ��� �̵� ���� �ʱ�ȭ
    }

    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveVec *= runSpeed / speed; // ������ ����
        }
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isDashing)
        {
            StartCoroutine(PerformDash());
        }
    }

    IEnumerator PerformDash()
    {
        isDashing = true;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            rb.MovePosition(rb.position + moveVec.normalized * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void StopToWall()
    {
        // �̵� ������ �������� ���̸� ��
        Debug.DrawRay(transform.position, moveVec.normalized * 1, Color.green);
        isBorder = Physics.Raycast(transform.position, moveVec.normalized, 1, LayerMask.GetMask("Wall"));

        if (isBorder)
        {
            moveVec = Vector3.zero; // ���� ������ �̵� ���͸� 0���� ����
        }
    }

    private void FixedUpdate()
    {
        StopToWall();
        if (!isDashing)
        {
            Move();
        }
    }

    void Move()
    {
        rb.MovePosition(rb.position + moveVec * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
