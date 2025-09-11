using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Ÿ�� ����")]
    public Transform target;

    [Header("ī�޶� �Ÿ� ����")]
    public float distance = 8;
    public float hight = 5;

    [Header("���콺 ����")]
    public float mouseSensivity = 2;
    public float minVerticalAngle = -30;
    public float maxVerticalAngle = 60;
    [Header("�ε巯�� ����")]
    public float positionSmoothTime = 0.2f;
    public float rotationSmoothTime = 0.1f;

    // ȸ����
    private float horizontalAngle;
    private float verticalAngle;
    // �����ӿ� ����
    private Vector3 currentVelocity;
    private Vector3 currentPosition;
    private Quaternion currentRotation;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else Debug.LogError("Player �±׸� ���� ������Ʈ ����");

        currentPosition = transform.position;
        currentRotation = transform.rotation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ToggleCursor();
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            HandleMouseInput();
            UpdateCameraSmooth();
        }
        else Debug.LogError("target ����");
    }

    void HandleMouseInput()
    {
        // Ŀ���� ������� ���� ���콺 �Է�
        if (Cursor.lockState != CursorLockMode.Locked) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity;

        horizontalAngle += mouseX;
        verticalAngle -= mouseY;

        verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle);
    }

    void UpdateCameraSmooth()   // ī�޶� ��ġ ���
    {
        //��ǥ ��ġ ���
        Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
        Vector3 rotateOffset = rotation * new Vector3(0, hight, -distance);
        Vector3 targetPosition = target.position + rotateOffset;

        Vector3 lookTarget = target.position + Vector3.up * hight;
        Quaternion targetRotation = Quaternion.LookRotation(lookTarget - targetPosition);

        currentPosition = Vector3.SmoothDamp(currentPosition, targetPosition, ref currentVelocity, positionSmoothTime);
        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime / rotationSmoothTime);

        transform.position = currentPosition;
        transform.rotation = currentRotation;
    }

    void ToggleCursor()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
