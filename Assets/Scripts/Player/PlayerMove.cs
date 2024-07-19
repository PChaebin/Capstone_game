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

    // ������ ���� ����
    public bool isRolling = false;
    public float rollSpeed = 15.0f;
    public float rollDuration = 3f;
    private float rollTimer = 0.0f;

    // ���� ���� ����
    public bool isAttacking = false;

    // ��ų ���� ����
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
        if (isAttacking || isSkilling) return; // ���� �Ǵ� ��ų ��� ���̸� �������� ����

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
            blendY = -0.75f; // ���� �̵�
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDirection = right;
            blendY = 0.75f; // ������ �̵�
        }
        else if (Input.GetKey(KeyCode.W))
        {
            moveDirection = forward;
            blendY = run ? 1 : 0.5f; // ������ �̵� (�޸��� �Ǵ� �ȱ�)
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDirection = -forward;
            blendY = 0; // �ڷ� �̵�
        }

        _animator.SetFloat("BlendY", blendY);

        if (_controller.isGrounded)
        {
            if (!isJumping)
            {
                verticalSpeed = -gravity * Time.deltaTime; // ���鿡 ���� �� �ణ�� �߷��� �����Ͽ� ���鿡 �پ� �ְ� ��
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
            verticalSpeed -= gravity * Time.deltaTime; // ���߿� ���� �� �߷� ����
        }

        moveDirection.y = verticalSpeed;

        // ������ ���� ��
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
        if (Input.GetMouseButtonDown(0)) // ��Ŭ������ ����
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) // ���� ��Ʈ�ѷ� ������
        {
            Roll();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) // ���� 1�� Ű�� ��ų ���
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
        yield return new WaitForSeconds(1.5f); // 1.5�� ���� ���� �ִϸ��̼� ���
        isAttacking = false;
    }

    IEnumerator SkillCooldown()
    {
        yield return new WaitForSeconds(3.5f); // 2�� ���� ��ų �ִϸ��̼� ���
        isSkilling = false;
    }

    public void OnAttackCollision()
    {
        attackCollision.SetActive(true);
    }
}