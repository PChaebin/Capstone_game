/*using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    CharacterController _controller;
    Animator _animator;
    Camera _camera;

    public float speed = 5.0f;
    public float runSpeed = 8.0f;
    public float jumpForce = 45.0f;
    public float gravity = 10.0f;
    private float verticalSpeed = 0.0f;
    public float finalSpeed;
    public bool run;
    public bool isJumping = false;

    public bool toggleCameraRotation;
    public float smoothness = 10.0f;

    // 구르기 관련 변수
    public bool isRolling = false;
    public float rollSpeed = 15.0f;
    public float rollDuration = 3f;
    private float rollTimer = 0.0f;

    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _camera = Camera.main;
        _controller = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            toggleCameraRotation = true;
        }
        else
        {
            toggleCameraRotation = false;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else
        {
            run = false;
        }

        HandleActions();
    }

    void FixedUpdate()
    {
        InputMovement();
    }

    void LateUpdate()
    {
        if (!toggleCameraRotation)
        {
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }
    }

    void InputMovement()
    {
        finalSpeed = run ? runSpeed : speed;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 moveDirection = (forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal")).normalized;

        if (_controller.isGrounded)
        {
            if (!isJumping)
            {
                verticalSpeed = -gravity * Time.deltaTime; // 지면에 있을 때 약간의 중력을 적용하여 지면에 붙어 있게 함
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    verticalSpeed = jumpForce;
                    _animator.SetFloat("Action", 1); // Jump
                    isJumping = true;
                }
            }
            else
            {
                isJumping = false;
                _animator.SetFloat("Action", 0); // 기본 상태로 복귀
            }
        }
        else
        {
            verticalSpeed -= gravity * Time.deltaTime; // 공중에 있을 때 중력 적용
        }

        moveDirection.y = verticalSpeed;

        // 구르기 중일 때
        if (isRolling)
        {
            rollTimer += Time.deltaTime;
            if (rollTimer < rollDuration)
            {
                _controller.Move(forward * rollSpeed * Time.deltaTime);
            }
            else
            {
                isRolling = false;
                rollTimer = 0.0f;
                _animator.SetFloat("Action", 0); // 기본 상태로 복귀
            }
        }
        else
        {
            _controller.Move(moveDirection * finalSpeed * Time.deltaTime);
        }

        float speedPercent = (run ? 1 : 0.5f) * moveDirection.magnitude;
        _animator.SetFloat("Speed", speedPercent, 0.1f, Time.deltaTime);

        // 디버그 로그 추가
        Debug.Log("Speed: " + speedPercent);
        Debug.Log("Action: " + _animator.GetFloat("Action"));
    }

    void HandleActions()
    {
        if (Input.GetMouseButtonDown(0)) // 좌클릭으로 공격
        {
            _animator.SetFloat("Action", 2); // Attack
            StartCoroutine(ResetActionAfterDelay(1.0f)); // 애니메이션 길이에 따라 지연시간 설정
            Debug.Log("Action set to Attack");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) // 왼쪽 컨트롤로 구르기
        {
            if (!isRolling)
            {
                isRolling = true;
                _animator.SetFloat("Action", 3); // Roll
                StartCoroutine(ResetActionAfterDelay(rollDuration)); // 구르기 길이에 따라 지연시간 설정
                Debug.Log("Action set to Roll");
            }
        }
    }

    IEnumerator ResetActionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _animator.SetFloat("Action", 0);
        Debug.Log("Action reset to Idle");
    }

    public void ResetAction()
    {
        _animator.SetFloat("Action", 0);
        Debug.Log("Action reset to Idle via animation event");
    }
}
*/