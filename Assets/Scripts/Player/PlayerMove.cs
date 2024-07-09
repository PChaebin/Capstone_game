using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;

    public float speed = 7.0f; // 일반 속도
    public float runSpeed = 10.0f; // 달리기 속도
    public float dashSpeed = 20.0f; // 대쉬 속도
    public float dashDuration = 0.2f; // 대쉬 지속 시간
    public float jumpForce = 5.0f; // 점프 힘

    private bool isDashing = false;
    private bool isGrounded = true;

    float hAxis;
    float vAxis;
    Vector3 moveVec;

    bool isBorder;

    public Transform cameraTransform; // 카메라 Transform을 받아옴

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

        forward.y = 0f; // 카메라의 y축 방향을 제거하여 수평 이동만 반영
        right.y = 0f;   // 카메라의 y축 방향을 제거하여 수평 이동만 반영

        forward.Normalize();
        right.Normalize();

        moveVec = forward * vAxis + right * hAxis;
        moveVec *= speed; // 기본 속도로 이동 벡터 초기화
    }

    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveVec *= runSpeed / speed; // 비율로 증가
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
        // 이동 벡터의 방향으로 레이를 쏨
        Debug.DrawRay(transform.position, moveVec.normalized * 1, Color.green);
        isBorder = Physics.Raycast(transform.position, moveVec.normalized, 1, LayerMask.GetMask("Wall"));

        if (isBorder)
        {
            moveVec = Vector3.zero; // 벽에 닿으면 이동 벡터를 0으로 설정
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
