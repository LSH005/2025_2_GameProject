using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�̵� ����")]
    public float walkSpeed = 3;
    public float runSpeed = 6;
    public float rotationSpeed = 10;

    [Header("���� ����")]
    public float jumpHight = 2;
    public float gravity = -9.81f;
    public float landingDuration = 0.3f;

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
    private bool isLanding = false;
    private float landingTimer;

    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGrounded;
    private float attackTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCam = Camera.main;
    }


    void Update()
    {
        CheckGrounded();
        HendleLanding();
        HandleMovement();
        UpdateAnimator();
        HendleAttack();
        HandleJump();

        
    }

    void HandleMovement()
    {
        // ���� or ���� �� ������ �� ��
        if ((isAttacking && !canMoveWhileAttacking) || isLanding)
        {
            currentSpeed = 0;
            return;
        }

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
        anim.SetBool("isGrounded", isGrounded);
        
        bool isFalling = !isGrounded && velocity.y < -0.1f;
        anim.SetBool("isFalling", isFalling);
        anim.SetBool("isLanding", isLanding);
    }

    void CheckGrounded()
    {
        wasGrounded = isGrounded;
        isGrounded = controller.isGrounded;

        if (!isGrounded && wasGrounded)
        {
            Debug.Log("�������� ����");
        }
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;

            if (!wasGrounded && anim != null)
            {
                isLanding = true;
                landingTimer = landingDuration;
            }
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHight * -2f * gravity);
            if (anim != null)
            {
                anim.SetTrigger("jumpTrigger");
            }
        }


        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void HendleLanding()
    {
        if (isLanding)
        {
            landingTimer -= Time.deltaTime;
            if (landingTimer <= 0)
            {
                isLanding = false;
            }
        }
    }

    void HendleAttack()
    {
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                isAttacking = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && !isAttacking)
        {
            isAttacking = true;
            attackTimer = attackDuration;
            if (anim != null)
            {
                anim.SetTrigger("attackTrigger");
            }
        }
    }
}
