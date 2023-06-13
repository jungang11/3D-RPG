using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float walkSpeed;   // 이동 속도
    [SerializeField] private float runSpeed;    // 이동 속도
    [SerializeField] private float crouchSpeed; // 앉기 속도
    [SerializeField] private float jumpForce;   // 점프 속도
    private float applySpeed;

    private CharacterController controller;
    private Animator anim;
    private Vector3 moveDir;
    private float ySpeed;

    // 상태 변수
    private bool isGrounded;
    private bool isWalking;
    public bool isCrouching;

    //앉았을 때 얼마나 앉을 지 결정하는 변수.
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        isCrouching = false;
        originPosY = Camera.main.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    private void OnEnable()
    {
        StartCoroutine(MoveRoutine());
        StartCoroutine(JumpRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            if (moveDir.magnitude == 0)
            {
                applySpeed = Mathf.Lerp(applySpeed, 0, 0.1f);
                anim.SetFloat("MoveSpeed", applySpeed);
                yield return null;
                continue;
            }

            if (isWalking)
                applySpeed = Mathf.Lerp(applySpeed, walkSpeed, 0.1f);
            else if (isCrouching)
                applySpeed = Mathf.Lerp(applySpeed, crouchSpeed, 0.1f);
            else
                applySpeed = Mathf.Lerp(applySpeed, runSpeed, 0.1f);

            Vector3 forwardVec = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
            Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

            controller.Move(forwardVec * moveDir.z * applySpeed * Time.deltaTime);
            controller.Move(rightVec * moveDir.x * applySpeed * Time.deltaTime);
            anim.SetFloat("MoveSpeed", applySpeed);

            Quaternion lookRotation = Quaternion.LookRotation(forwardVec * moveDir.z + rightVec * moveDir.x);
            transform.rotation = Quaternion.Lerp(lookRotation, transform.rotation, 0.1f);
            yield return null;
        }
    }

    private void OnMove(InputValue value)
    {
        moveDir.x = value.Get<Vector2>().x;
        moveDir.z = value.Get<Vector2>().y;
    }

    private void OnWalk(InputValue value)
    {
        if(!isCrouching)
            isWalking = value.isPressed;
    }

    private IEnumerator JumpRoutine()
    {
        while (true)
        {
            // 아래 방향으로 계속해서 중력을 받음
            ySpeed += Physics.gravity.y * Time.deltaTime;

            if (isGrounded && ySpeed < 0)
                ySpeed = 0;

            controller.Move(Vector3.up * ySpeed * Time.deltaTime);

            yield return null;
        }
    }

    private void OnJump(InputValue value)
    {
        if (isCrouching)
        {
            isCrouching = false;
            applySpeed = walkSpeed;
            anim.SetBool("Crouching", false);
            controller.center = new Vector3(0f, 1f, 0f);
            controller.height = 1.8f;
            return;
        }
        anim.SetTrigger("IsJump");
        ySpeed = jumpForce;
    }

    public IEnumerator CrouchRoutine()
    {
        if (isCrouching)
        {
            applySpeed = crouchSpeed;
            anim.SetBool("Crouching", true);
            controller.center = new Vector3(0f, 0.8f, 0f);
            controller.height = 1.4f;
        }
        else
        {
            applySpeed = walkSpeed;
            anim.SetBool("Crouching", false);
            controller.center = new Vector3(0f, 1f, 0f);
            controller.height = 1.8f;
        }
        yield return null;
    }

    private void OnCrouch(InputValue value)
    {
        isCrouching = !isCrouching;

        StartCoroutine(CrouchRoutine());
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        isGrounded = Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.5f);
    }
}
