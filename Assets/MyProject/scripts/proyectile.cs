using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proyectile : MonoBehaviour
{
    public float speed = .1f;
    public bool p;
    public Rigidbody rb;

    void Update()
    {
        if (!p)
        {
            float framedistance = speed * Time.deltaTime;
            transform.Translate(transform.forward * framedistance, Space.World);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }
}
