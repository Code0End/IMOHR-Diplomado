using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debris : MonoBehaviour
{
    void Awake()
    {
        Invoke(nameof(apagar_debris), 5f);
    }

    private void apagar_debris()
    {
        Rigidbody[] rbl = GetComponentsInChildren<Rigidbody>();
        MeshCollider[] mcl = GetComponentsInChildren<MeshCollider>();

        foreach (Rigidbody rb in rbl)
        {
            rb.isKinematic = true;
        }

        foreach (MeshCollider mc in mcl)
        {
            mc.enabled = false;
        }
    }

}
