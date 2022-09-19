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

    public bool p,t,m;

    private void Awake()
    {
        if (m) Invoke(nameof(destruir), 0.2f);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (pattack)
        {
            sc.enabled = false;
            rb.isKinematic = true;
        }
        else
        {
            bc.enabled = false;
        }
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<character>().taked(damage);
        }

        if (!m) { 
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
            Invoke(nameof(destruir),0.1f);
        }
    }

    public void destruir()
    {
        Destroy(gameObject);
    }
}
