using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seguir_camara : MonoBehaviour
{
    [SerializeField] private Transform camera_pos;

    void Update()
    {
        transform.position = camera_pos.position;
    }
}
