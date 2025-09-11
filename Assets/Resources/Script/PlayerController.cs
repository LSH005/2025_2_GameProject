using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 3;
    public float runSpeed = 6;
    public float rotationSpeed = 10;

    [Header("공격 설정")]
    public float attackDuration = 0.8f; // 공격 지속시간
    public bool canMoveWhileAttacking = false;  //공격 중 이동 가능?

    [Header("컴포넌트")]
    public Animator anim;


    // 컴포넌트 참조
    private CharacterController controller;
    private Camera playerCam;
    // 현재 상태값
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
            // 카메라가 보는 방향의 앞쪽으로 방향 지정
            Vector3 cameraFoward = playerCam.transform.forward;
            Vector3 cameraRight = playerCam.transform.right;
            cameraFoward.y = cameraRight.y = 0;
            cameraFoward.Normalize();
            cameraRight.Normalize();

            // 이동 방향 설정
            Vector3 MoveDirection = (cameraFoward * vertical) + (cameraRight * horizontal);

            // 달리기
            if (Input.GetKey(KeyCode.LeftShift)) currentSpeed = runSpeed;
            else currentSpeed = walkSpeed;

            controller.Move(MoveDirection * currentSpeed * Time.deltaTime);

            // 이동 진행 방향을 향하여 바라보도록 Slerp (각도 선형 보간)
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
