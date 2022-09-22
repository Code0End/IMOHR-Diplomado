using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eattack : MonoBehaviour
{
    public int damage = 25;

    [SerializeField] private AudioSource[] a_s;
    [SerializeField] private AudioClip[] clips1;

    public GameObject proyectile, impact;

    public bool pattack;
    public Rigidbody rb;
    public SphereCollider sc;
    public BoxCollider bc;
    public Light L;

    public bool p,t,m,g,dz,w;

    private void Awake()
    {
        if (m) Invoke(nameof(destruir), 0.2f);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (w)
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<character>().cambiar_cf();
                other.gameObject.GetComponent<character>().w = true;
            }
        }
        else
        {
            if (!dz)
            {
                if (pattack)
                {
                    L.enabled = false;
                    sc.enabled = false;
                    rb.isKinematic = true;

                }
                else
                {
                    bc.isTrigger = true;
                    bc.enabled = false;
                }

                if (other.gameObject.tag == "Player")
                {
                    if (!g)
                        other.gameObject.GetComponent<character>().taked(damage);
                    else
                        if (other.gameObject.GetComponent<controlador_movimiento>().suelo)
                        other.gameObject.GetComponent<character>().taked(damage);

                }

                if (!m)
                {
                    if (other.gameObject.tag == "shield")
                    {
                        if (t)
                            other.gameObject.GetComponentInParent<shield1>().on_defend();
                        else
                            Debug.Log("xd");
                    }

                    a_s[0].Stop();
                    if (!p)
                    {
                        a_s[0].pitch = Random.Range(0.85f, 1.2f);
                        a_s[0].volume = 0.15f;
                        a_s[0].PlayOneShot(clips1[0]);
                    }
                    else
                    {
                        a_s[0].pitch = Random.Range(0.6f, 1f);
                        a_s[0].volume = 0.15f;
                        a_s[0].PlayOneShot(clips1[0]);
                    }
                    proyectile.SetActive(false);
                    impact.SetActive(true);
                    Invoke(nameof(destruir), 4f);
                }
                else
                {
                    a_s[0].pitch = Random.Range(0.85f, 1.2f);
                    a_s[0].volume = 0.5f;
                    a_s[0].PlayOneShot(clips1[0]);
                    proyectile.SetActive(false);
                    Invoke(nameof(destruir), 0.1f);
                }
            }
            else
            {
                if (other.gameObject.tag == "Player")
                {
                    other.gameObject.GetComponent<character>().taked(damage);
                }
                if (other.gameObject.tag == "enemy1")
                {
                    if (other.gameObject.GetComponentInChildren<enemy>() == null) return;
                    other.gameObject.GetComponentInChildren<enemy>().dz = true;
                    other.gameObject.GetComponentInChildren<enemy>().taked(damage);
                }
            }
        }
    }

    public void destruir()
    {
        Destroy(gameObject);
    }
}
