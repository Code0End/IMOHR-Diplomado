using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move1 : MonoBehaviour
{

    public float speed = 10f;
    public Rigidbody pro_rigid;

    void Start()
    {
        //pro_rigid.velocity = transform.right * speed;
    }

    void Update()
    {
        pro_rigid.velocity = -transform.forward * speed;
    }
}
