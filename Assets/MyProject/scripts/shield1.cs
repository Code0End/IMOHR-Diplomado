using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield1 : MonoBehaviour
{
    public GameObject Shield;
    public arma_sway sway;
    public bool defending = false;
    bool impact,d;

    Animator anime;

    public BoxCollider bc;
    Rigidbody rb;

    private void Start()
    {
        anime = Shield.GetComponent<Animator>();
        bc.enabled = false;
    }

    void Update()
    {
        if (d) return;
        if (Input.GetMouseButton(1))
        {
            shielddefend(true);
            return;
        }
        if (Input.GetMouseButtonUp(1))
        {
            if(!impact)
                shielddefend(false);
            return;
        }
        if(!Input.GetMouseButton(1) && !Input.GetMouseButtonUp(1) && (!impact))
                shielddefend(false);
    }

    public void shielddefend(bool e)
    {
        Animator anim = Shield.GetComponent<Animator>();
        if (e == true)
        {
            bc.enabled = true;
            defending = true;
            sway.multiplicador_frente = 0f;
            anim.SetBool("defend", true);
        }
        else
        {
            bc.enabled = false;
            defending = false;
            sway.multiplicador_frente = 30f;
            anim.SetBool("defend", false);
        }
    }

    public void on_defend()
    {
            impact = true;
            StartCoroutine(reset_i(1));         
            anime.SetTrigger("knockback");        
    }

    IEnumerator reset_i(float t)
    {
        yield return new WaitForSeconds(t);
        impact = false;
    }

    public void drop_shield()
    {
        d = true;
        rb = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        bc = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
        bc.enabled = true;
        rb.transform.parent = null;
        rb.isKinematic = false;
        SetLayerRecursively(gameObject, 13);
        Destroy(gameObject.GetComponent<arma_sway>());
        Destroy(gameObject.GetComponent<shield1>());
    }

    public static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
}
