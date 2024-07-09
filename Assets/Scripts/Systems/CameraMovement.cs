using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform objectTofollow;
    public float followSpeed = 10f;
    public float sensitivity = 100f;
    public float clampAngle = 70f;

    private float rotX;
    private float rotY;

    public Transform realCamera;
    public Vector3 offset;
    public float smoothness = 10f;

    void Start()
    {
        // objectTofollow와 realCamera가 설정되지 않은 경우 기본 값으로 할당
        if (objectTofollow == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                objectTofollow = player.transform;
            }
            else
            {
                Debug.LogError("Player not found in the scene. Please assign the Player object.");
            }
        }

        if (realCamera == null)
        {
            realCamera = Camera.main.transform;
            Debug.Log("Real Camera automatically assigned to Main Camera");
        }

        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        offset = realCamera.position - objectTofollow.position; // 초기 오프셋 계산
    }

    void Update()
    {
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }

    void LateUpdate()
    {
        if (objectTofollow == null || realCamera == null) return;

        Vector3 targetPosition = objectTofollow.position + offset;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);

        RaycastHit hit;
        if (Physics.Linecast(transform.position, targetPosition, out hit))
        {
            realCamera.position = hit.point;
        }
        else
        {
            realCamera.position = Vector3.Lerp(realCamera.position, targetPosition, Time.deltaTime * smoothness);
        }
    }
}
