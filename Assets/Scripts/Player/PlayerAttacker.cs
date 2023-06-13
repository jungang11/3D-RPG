using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] Weapon weapon;


    private Animator anim;
    private PlayerMover player;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<PlayerMover>();
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }

    private void OnAttack(InputValue value)
    {
        if (player.isCrouching)
        {
            player.isCrouching = false;
            StartCoroutine(player.CrouchRoutine());
            return;
        }
        Attack();
    }

    public void StartAttack()
    {
        weapon.EnableWeapon();
        Debug.Log("공격 시작");
    }

    public void EndAttack()
    {
        weapon.DisableWeapon();
        Debug.Log("공격 끝");
    }
}