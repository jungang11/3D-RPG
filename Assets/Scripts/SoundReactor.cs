using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundReactor : MonoBehaviour, IListenable
{
    public void Listen(Transform trans)
    {
        StartCoroutine(LookAtRoutine(trans));
    }

    IEnumerator LookAtRoutine(Transform trans)
    {
        //Quaternion lookRotation = Quaternion.LookRotation(trans.transform.position);
        //transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.5f);
        transform.LookAt(trans.transform.position);

        yield return null;
    }
}
