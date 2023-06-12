using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;   // 이동 속도
    [SerializeField] private float runSpeed;   // 이동 속도
    [SerializeField] private float jumpForce;   // 점프 속도
    private float applySpeed;

    private CharacterController controller;
    private Animator anim;
    private Vector3 moveDir;
    private float ySpeed;

    private bool isGrounded;
    private bool isWalking;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        Fall();
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void Move()
    {
        if (moveDir.magnitude == 0)
        {
            applySpeed = Mathf.Lerp(applySpeed, 0, 0.1f);
            anim.SetFloat("MoveSpeed", applySpeed);
            return;
        }

        if (isWalking)
        {
            applySpeed = Mathf.Lerp(applySpeed, walkSpeed, 0.1f);
        }
        else
        {
            applySpeed = Mathf.Lerp(applySpeed, runSpeed, 0.1f);
        }

        Vector3 forwardVec = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;
        
        controller.Move(forwardVec * moveDir.z * applySpeed * Time.deltaTime);
        controller.Move(rightVec * moveDir.x * applySpeed * Time.deltaTime);
        anim.SetFloat("MoveSpeed", applySpeed);

        Quaternion lookRotation = Quaternion.LookRotation(forwardVec * moveDir.z + rightVec * moveDir.x);
        transform.rotation = Quaternion.Lerp(lookRotation, transform.rotation, 0.1f);
    }

    private void OnMove(InputValue value)
    {
        moveDir.x = value.Get<Vector2>().x;
        moveDir.z = value.Get<Vector2>().y;
    }

    private void OnWalk(InputValue value)
    {
        isWalking = value.isPressed;
    }

    private void Fall()
    {
        // 아래 방향으로 계속해서 중력을 받음
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (isGrounded && ySpeed < 0)
            ySpeed = 0;

        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }

    private void Jump()
    {
        ySpeed = jumpForce;
    }

    private void OnJump(InputValue value)
    {
        anim.SetTrigger("IsJump");
        Jump();
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        // SphereCast -> 직선 레이저가 아니라 원 모양 레이저로 판단
        isGrounded = Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.6f);
    }
}
