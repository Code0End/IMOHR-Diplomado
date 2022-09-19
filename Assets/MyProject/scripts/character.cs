using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using DG.Tweening;

public class character : MonoBehaviour
{
    public float maxhp = 100;
    public float hp = 100;
    public float maxstamina = 100;
    public float stamina = 100;
    public float staminaregen = 20;

    public barras bhp;
    public barras bst;

    [SerializeField] private AudioSource[] a_s;
    [SerializeField] private AudioClip[] clips1;

    public Volume volumen;
    private Vignette v;

    float f = 0.2f;
    int a=0;

    private void Start()
    {
        volumen.profile.TryGet<Vignette>(out v);
    }

    public void usarstamina(int cantidad)
    {
        if (stamina > 0)
        {
            stamina -= cantidad;
            bst.updatestamina(stamina / maxstamina);
        }
    }

    public void ganarstamina(int cantidad)
    {
            stamina += cantidad * Time.deltaTime;
            bst.updatestamina(stamina / maxstamina);
    }

    public float checkstamina()
    {
        return stamina;
    }

    void Update()
    {
        if (hp < maxhp)
        {
            v.intensity.value = f;
        }
        if (stamina < maxstamina)
        {
            ganarstamina(10);
        }
    }

    public void taked(float d)
    {
        hp -= d;
        bst.updatehp(hp / maxhp);
        if (hp <= 0)
        {
            a_s[0].pitch = Random.Range(0.85f, 1.2f);
            a_s[0].PlayOneShot(clips1[1]);
            Debug.Log("MUERTO");
        }
        else
        {
            a=1;
            cambiar_vi();
            a_s[0].pitch = Random.Range(0.85f, 1.2f);
            a_s[0].PlayOneShot(clips1[0]);
        }
    }

    public void cambiar_vi()
    {
        DOTween.To(() => f, x => f = x, 0.6f, 0.2f);
        if (a==1)
        {
            a = 2;
            Invoke(nameof(r_v), 5f);
        }
    }

    public void r_v()
    {
        if (a==2)
        {
            DOTween.To(() => f, x => f = x, 0.2f, 1f); ;
            a = 0;
        }
        else
        {
            a = 2;
            Invoke(nameof(r_v), 5f);
            return;
        }
    }
}
