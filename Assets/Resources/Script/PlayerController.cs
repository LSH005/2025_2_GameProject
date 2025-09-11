using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("�̵� ����")]
    public float walkSpeed = 3;
    public float runSpeed = 6;
    public float rotationSpeed = 10;

    [Header("���� ����")]
    public float attackDuration = 0.8f; // ���� ���ӽð�
    public bool canMoveWhileAttacking = false;  //���� �� �̵� ����?

    [Header("������Ʈ")]
    public Animator anim;


    // ������Ʈ ����
    private CharacterController controller;
    private Camera playerCam;
    // ���� ���°�
    private float currentSpeed;
    private bool isAttacking;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCam = Camera.main;
    }


    void Update()
    {
        HandleMovement();
        UpdateAnimator();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            // ī�޶� ���� ������ �������� ���� ����
            Vector3 cameraFoward = playerCam.transform.forward;
            Vector3 cameraRight = playerCam.transform.right;
            cameraFoward.y = cameraRight.y = 0;
            cameraFoward.Normalize();
            cameraRight.Normalize();

            // �̵� ���� ����
            Vector3 MoveDirection = (cameraFoward * vertical) + (cameraRight * horizontal);

            // �޸���
            if (Input.GetKey(KeyCode.LeftShift)) currentSpeed = runSpeed;
            else currentSpeed = walkSpeed;

            controller.Move(MoveDirection * currentSpeed * Time.deltaTime);

            // �̵� ���� ������ ���Ͽ� �ٶ󺸵��� Slerp (���� ���� ����)
            Quaternion targetRotation = Quaternion.LookRotation(MoveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else currentSpeed = 0;
    }

    void UpdateAnimator()
    {
        float animatorSpeed = Mathf.Clamp01(currentSpeed / runSpeed);
        anim.SetFloat("speed", animatorSpeed);
    }
}
