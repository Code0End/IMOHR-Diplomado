using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class enemy : MonoBehaviour
{

    public bool isdoor;
    public bool di;
    public GameObject corpse;

    public int maxhp = 100;
    public int hp = 100;

    [SerializeField] private AudioSource[] a_s;
    [SerializeField] private AudioClip[] clips1;

    public enemy_comportamiento ec;
    public NavMeshAgent agent;

    GameObject gamem;

    public bool dz;

    public GameObject[] vfx_r;


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "sword1")
        {
            //if (isdoor = !true) return;
            if (other.gameObject.GetComponent<espada_movimiento>().atacado) return;
            taked(other.gameObject.GetComponent<espada_movimiento>().damage);
            Quaternion contact_r = Quaternion.FromToRotation(Vector3.up, other.contacts[0].point);
            Vector3 contact_p = other.contacts[0].point;
            Instantiate(vfx_r[0], contact_p, contact_r);
            Invoke(nameof(destroy_vfx), 2f);
        }
    }

    public void taked(int d)
    {
        hp = hp - d;
        //hb.UpdateHealth(hp / maxhp);
        if (hp <= 0)
        {          
            a_s[0].pitch = Random.Range(0.7f, 1.4f);
            a_s[0].PlayOneShot(clips1[0]);
            if (!di)
            {
                if (!dz)
                {                 
                    ec.CancelInvoke();
                    ec.StopAllCoroutines();
                    agent.enabled = false;
                    ec.enabled = false;
                    gamem = GameObject.Find("gamemanager");
                    gamem.GetComponent<manejador_gameplay>().enemigos_derrotados++;
                    gameObject.SetActive(false);
                    corpse.SetActive(true);
                    
                }
                else
                {
                    agent.enabled = false;
                    ec.CancelInvoke();
                    ec.StopAllCoroutines();
                    ec.Drb();
                    ec.enabled = false;
                    gamem = GameObject.Find("gamemanager");
                    gamem.GetComponent<manejador_gameplay>().enemigos_derrotados++;
                    gameObject.SetActive(false);
                    corpse.SetActive(true);

                }
            }
            else
            {
                gameObject.SetActive(false);
                corpse.SetActive(true);
            }
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
                if (!isdoor)
                {
                    if (other.gameObject.GetComponent<espada_movimiento>().atacado) return;
                    taked(other.gameObject.GetComponent<espada_movimiento>().damage);
                    other.gameObject.GetComponent<espada_movimiento>().atacado = true;                    
                    var collisionpoint = other.ClosestPoint(transform.position);
                    var collisionnormal = transform.position - collisionpoint;
                    Quaternion quaternioon = Quaternion.Euler(collisionnormal.x, collisionnormal.y, collisionnormal.z);
                    Instantiate(vfx_r[1], collisionpoint, quaternioon);
                
                    Invoke(nameof(destroy_vfx), 2f);
            }
                else
                {
                    taked(other.gameObject.GetComponent<espada_movimiento>().damage);
                    var collisionpoint = other.ClosestPoint(transform.position);
                    var collisionnormal = transform.position - collisionpoint;
                    Quaternion quaternioon = Quaternion.Euler(collisionnormal.x, collisionnormal.y, collisionnormal.z);
                    Instantiate(vfx_r[1], collisionpoint, quaternioon);

                    Invoke(nameof(destroy_vfx), 2f);
                }
            }
        }

    public void walksound()
    {
        a_s[0].pitch = Random.Range(0.3f, 1.5f);
        a_s[0].PlayOneShot(clips1[2]);
    }

    public void destroy_vfx(GameObject vfx)
    {
        Destroy(vfx);
    }

}
