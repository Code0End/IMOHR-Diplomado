using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{

    private bool isdoor;
    public bool di;
    public GameObject corpse;

    public int maxhp = 100;
    public int hp = 100;

    [SerializeField] private AudioSource[] a_s;
    [SerializeField] private AudioClip[] clips1;

    public enemy_comportamiento ec;


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "sword1")
        {
            //if (isdoor = !true) return;
            if (other.gameObject.GetComponent<espada_movimiento>().atacado) return;
            taked(other.gameObject.GetComponent<espada_movimiento>().damage);
        }
    }

    public void taked(int d)
    {
        hp = hp - d;
        //hb.UpdateHealth(hp / maxhp);
        if (hp <= 0)
        {
            a_s[0].pitch = Random.Range(0.85f, 1.2f);
            a_s[0].PlayOneShot(clips1[0]);
            if (!di)
            {
                ec.CancelInvoke();
                ec.StopAllCoroutines();
                ec.enabled = false;
            }
            gameObject.SetActive(false);
            corpse.SetActive(true);

        }
        else
        {
            a_s[0].pitch = Random.Range(0.85f, 1.2f);
            a_s[0].PlayOneShot(clips1[1]);
        }
    }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "sword1")
            {
            if (isdoor = !true) return;
            if (other.gameObject.GetComponent<espada_movimiento>().atacado) return;
                taked(other.gameObject.GetComponent<espada_movimiento>().damage);
                other.gameObject.GetComponent<espada_movimiento>().atacado = true;
            }
    }

    public void walksound()
    {
        a_s[0].pitch = Random.Range(0.6f, 1f);
        a_s[0].PlayOneShot(clips1[2]);
    }

}
