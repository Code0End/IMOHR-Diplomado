using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class character : MonoBehaviour
{
    public float maxhp = 100;
    public float hp = 100;
    public float maxstamina = 100;
    public float stamina = 100;
    public float staminaregen = 20;
    public bool w;

    public barras bhp;
    public barras bst;

    [SerializeField] private AudioSource[] a_s;
    [SerializeField] private AudioClip[] clips1;
    [SerializeField] private sword1 sword;
    [SerializeField] private shield1 shield;
    [SerializeField] private BoxCollider shieldbc;
    [SerializeField] private Image[] images;
    [SerializeField] private controlador_camara ccamara;
    [SerializeField] private GameObject[] ui_derrotado;

    public Volume volumen;
    private Vignette v;
    private ColorAdjustments s;    

    float f=0.2f,f2=0f,f3=1f,f4=1f,f5=1f;
    Rigidbody rb;
    int a=0;

    private void Start()
    {
        volumen.profile.TryGet<Vignette>(out v);
        volumen.profile.TryGet<ColorAdjustments>(out s);
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
        if (hp <= 0)
        {
            s.saturation.value = f2;
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }
        if (stamina < maxstamina)
        {
            ganarstamina(10);
        }
        if (w)
        {
            //var color = new Color(f3, f4, f5);
            Color color = new Vector4(f3, f4, f5, 1.0f);
            s.colorFilter.value = color;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
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
            cambiar_sa();
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

    public void cambiar_cf()
    {
        DOTween.To(() => f3, x => f3 = x, 0f, 6f);
        DOTween.To(() => f4, x => f4 = x, 0f, 6f);
        DOTween.To(() => f5, x => f5 = x, 0f, 6f);

        Destroy(gameObject.GetComponent<controlador_movimiento>());
        Destroy(images[3]);
        Destroy(images[2]);
        Destroy(images[1]);
        Destroy(images[0]);

        ui_derrotado[3].SetActive(true);
        ui_derrotado[4].SetActive(true);
        ui_derrotado[2].SetActive(true);

        sword.drop_sword();
        shield.drop_shield();
        SetLayerRecursively(gameObject, 0);
        gameObject.tag = "Untagged";
        rb = gameObject.GetComponent<Rigidbody>();
        ccamara.d = true;
    }

    public void cambiar_sa()
    {
        DOTween.To(() => f2, x => f2 = x, -100f, 1f);
        Destroy(gameObject.GetComponent<controlador_movimiento>());
        sword.drop_sword();
        shield.drop_shield();
        SetLayerRecursively(gameObject, 0);
        gameObject.tag = "Untagged";
        Destroy(images[3]);
        Destroy(images[2]);
        Destroy(images[1]);
        Destroy(images[0]);
        ccamara.d = true;
        ui_derrotado[0].SetActive(true);
        ui_derrotado[1].SetActive(true);
        ui_derrotado[2].SetActive(true);
        rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
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
    public static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
}
