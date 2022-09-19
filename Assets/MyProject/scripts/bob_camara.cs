using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bob_camara : MonoBehaviour
{
    [SerializeField, Range(0, 0.1f)] private float amplitud = 0.015f;
    [SerializeField, Range(0, 30)] private float frecuencia = 10.0f;

    [SerializeField] private Transform camara;
    [SerializeField] private Transform padrecamara;

    private float velocidadcambio = 3.0f;
    private Vector3 posini;
    [SerializeField] private controlador_movimiento controlador;

    [SerializeField] private AudioSource[] a_s;
    [SerializeField] private AudioClip[] clips1;

    public Vector3 poso = Vector3.zero;

    private void Awake()
    {
        posini = camara.localPosition;
    }
    void Update()
    {
        revisarmov();
        reiniciar();
        //camara.LookAt(ver());
    }

    private Vector3 pasos()
    {
        poso = Vector3.zero;
        poso.y += Mathf.Sin(Time.time * frecuencia) * amplitud;
        poso.x += Mathf.Cos(Time.time * frecuencia/2) * amplitud*2;
        if (poso.x <= -0.005)
        {
            if (!a_s[0].isPlaying)
            {
                a_s[0].pitch = Random.Range(0.85f, 1.2f);
                a_s[0].PlayOneShot(clips1[0]);
                return poso;
            }
        }
        if (poso.x >= 0.005)
        {
            if (!a_s[1].isPlaying)
            {
                a_s[1].pitch = Random.Range(0.85f, 1.2f);
                a_s[1].PlayOneShot(clips1[0]);
                return poso;
            }
        }
        return poso;
    }

    private void revisarmov()
    {
        float velocidad = new Vector3(controlador.rb.velocity.x, 0, controlador.rb.velocity.z).magnitude;

        if (velocidad < velocidadcambio) return;
        if (!controlador.suelo) return;
        if (controlador.deslizando) return;

        hacermov(pasos());
    }

    private void hacermov(Vector3 mov)
    {
        camara.localPosition += mov;
    }

    private void reiniciar()
    {
        if (camara.localPosition == posini) return;
        camara.localPosition = Vector3.Lerp(camara.localPosition, posini, 1 * Time.deltaTime);
    }

    private Vector3 ver()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + padrecamara.localPosition.y, transform.position.z);
        pos += padrecamara.forward * 15f;
        return pos;
    }
}
