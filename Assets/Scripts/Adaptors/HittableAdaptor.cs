using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HittableAdaptor : MonoBehaviour, IHittable
{
    public UnityEvent<int> OnHittable;

    public void TakeHit(int damage)
    {
        OnHittable?.Invoke(damage);
    }
}
