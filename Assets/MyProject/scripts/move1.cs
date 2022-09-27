using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move1 : MonoBehaviour
{
    public Transform[] tr;
    Vector3 start, end;

    void OnEnable()
    {

        transform.position = tr[1].position;
        Invoke(nameof(ddd), 1.3f);
    }

    void Update()
    {
        
        transform.position = tr[1].position;
    }

    void ddd()
    {
        Destroy(this);
    }
}
