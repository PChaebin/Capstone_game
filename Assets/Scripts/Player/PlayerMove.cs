using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    CharacterController _controller;
    Animator _animator;
    Camera _camera;
    public GameObject attackCollision;

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

    // 공격 관련 변수
    public bool isAttacking = false;

    // 스킬 관련 변수
    public bool isSkilling = false;

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
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }
    }

    void InputMovement()
    {
        if (isAttacking || isSkilling) return; // 공격 또는 스킬 사용 중이면 움직임을 멈춤

        finalSpeed = run ? runSpeed : speed;

        Vector3 forward = _camera.transform.TransformDirection(Vector3.forward);
        Vector3 right = _camera.transform.TransformDirection(Vector3.right);

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = Vector3.zero;
        float blendY = 0;

        if (Input.GetKey(KeyCode.A))
        {
            moveDirection = -right;
            blendY = -0.75f; // 왼쪽 이동
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDirection = right;
            blendY = 0.75f; // 오른쪽 이동
        }
        else if (Input.GetKey(KeyCode.W))
        {
            moveDirection = forward;
            blendY = run ? 1 : 0.5f; // 앞으로 이동 (달리기 또는 걷기)
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDirection = -forward;
            blendY = 0; // 뒤로 이동
        }

        _animator.SetFloat("BlendY", blendY);

        if (_controller.isGrounded)
        {
            if (!isJumping)
            {
                verticalSpeed = -gravity * Time.deltaTime; // 지면에 있을 때 약간의 중력을 적용하여 지면에 붙어 있게 함
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    verticalSpeed = jumpForce;
                    _animator.SetTrigger("isJumping");
                    isJumping = true;
                }
            }
            else
            {
                isJumping = false;
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
            HandleRolling(forward);
        }
        else
        {
            _controller.Move(moveDirection * finalSpeed * Time.deltaTime);
        }
    }

    void HandleActions()
    {
        if (Input.GetMouseButtonDown(0)) // 좌클릭으로 공격
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) // 왼쪽 컨트롤로 구르기
        {
            Roll();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) // 숫자 1번 키로 스킬 사용
        {
            UseSkill();
        }
    }

    void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            _animator.SetTrigger("isAttacking");
            StartCoroutine(AttackCooldown());
        }
    }

    void Roll()
    {
        if (!isRolling)
        {
            isRolling = true;
            _animator.SetTrigger("isRolling");
        }
    }

    void UseSkill()
    {
        if (!isSkilling)
        {
            isSkilling = true;
            _animator.SetTrigger("isSkilling");
            StartCoroutine(SkillCooldown());
        }
    }

    void HandleRolling(Vector3 forward)
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
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1.5f); // 1.5초 동안 공격 애니메이션 재생
        isAttacking = false;
    }

    IEnumerator SkillCooldown()
    {
        yield return new WaitForSeconds(3.5f); // 2초 동안 스킬 애니메이션 재생
        isSkilling = false;
    }

    public void OnAttackCollision()
    {
        attackCollision.SetActive(true);
    }
}