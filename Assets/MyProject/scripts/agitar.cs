using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agitar : MonoBehaviour
{
    public GameObject padrecam;

    //agitar cámara
    public bool agitar_activo = true;
    [Range(0, 1)][SerializeField] float trauma;
    [SerializeField] float traumam = 5f;
    [SerializeField] float traumamag = 0.8f;
    [SerializeField] float traumarotmag = 1.5f;
    [SerializeField] float traumaprofmag = 1.3f;
    [SerializeField] float traumadis = 1.3f;
    float timer;

    void Start()
    {
        
    }


    void Update()
    {
        if (agitar_activo && Truama > 0)
        {
            timer += Time.deltaTime * Mathf.Pow(trauma,0.3f) * traumam;
            Vector3 newpos = obtener_vec3() * traumamag*Truama;
            transform.localPosition = newpos;
            transform.localRotation = Quaternion.Euler(newpos*traumarotmag);
            Truama -= Time.deltaTime * traumadis * Truama;
        }
        else
        {
            Vector3 newpos = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime);
            transform.localPosition = newpos;
            transform.localRotation = Quaternion.Euler(newpos * traumarotmag);
        }
    }

    public float Truama
    {
        get { return trauma; }

        set { trauma = Mathf.Clamp01(value); }
    }

    float obtener_float(float semilla)
    {
        return (Mathf.PerlinNoise(semilla, timer) - 0.5f) * 2;
    }

    Vector3 obtener_vec3()
    {
        return new Vector3(obtener_float(1), obtener_float(10), obtener_float(100)*traumaprofmag);
    }
}
