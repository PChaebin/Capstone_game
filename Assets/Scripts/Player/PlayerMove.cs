using System.Collections;
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

    // ������ ���� ����
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
        else
        {
            _controller.Move(moveDirection * finalSpeed * Time.deltaTime);
        }

        float percent = ((run) ? 1 : 0.5f) * moveDirection.magnitude;
        _animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }

    void HandleActions()
    {
        if (Input.GetMouseButtonDown(0)) // ��Ŭ������ ����
        {
            _animator.SetTrigger("isAttacking");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) // ���� ��Ʈ�ѷ� ������
        {
            if (!isRolling)
            {
                isRolling = true;
                _animator.SetTrigger("isRolling");
            }
        }
    }
}