using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int damage;

    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    public void EnableWeapon()
    {
        col.enabled = true;
    }

    public void DisableWeapon()
    {
        col.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        IHittable hittable = other.GetComponent<IHittable>();
        hittable?.TakeHit(damage);
    }
}
