using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class espada_movimiento : MonoBehaviour
{
    public bool activada;
    public bool girando;
    public bool dsword;
    public float velocidad_rotacion;
    public bool atacado;

    [SerializeField] private Rigidbody rb;
    public AudioSource[] a_s;
    [SerializeField] private AudioClip[] clips1;

    public int damage = 25;

    const string enemy = "enemy1";
    const string proyectile = "eattack";
    const string end_zone = "ez";

    void FixedUpdate()
    {
        if (girando)
        {
            Vector3 localforward = transform.worldToLocalMatrix.MultiplyVector(transform.forward);
            transform.localEulerAngles +=  -localforward * velocidad_rotacion * Time.deltaTime;
        }
        if (!activada) return;
            transform.rotation = (Quaternion.LookRotation(rb.velocity)*Quaternion.Euler(0, -90, 0));
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!dsword) return;
        var e = collision.gameObject.tag;
        if (e == enemy|| e==proyectile)
        {

        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
            activada = false;
            StopAllCoroutines();
        }
    }

    public void play_sound()
    {
        a_s[0].pitch = Random.Range(0.85f, 1.2f);
        a_s[0].PlayOneShot(clips1[0]);
    }

    public void stop_sound()
    {
        a_s[0].Stop();
    }

    public void stop_running()
    {
        StartCoroutine(reset_f(0.02f));
    }

    IEnumerator reset_f(float t)
    {
        yield return new WaitForSeconds(t);
        activada = true;
    }
}
