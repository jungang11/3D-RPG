using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public void Up()
    {
        transform.position = new Vector3(0, 3f, 0); 
    }
}
