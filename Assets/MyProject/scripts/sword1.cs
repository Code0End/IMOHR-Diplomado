using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class sword1 : MonoBehaviour
{
    public GameObject Sword;
    public shield1 shield;
    public bool can_attack = true;
    private int air = 0;
    public controlador_movimiento mov;
    public controlador_camara cam;

    [SerializeField] private AudioSource[] a_s;
    [SerializeField] private AudioClip[] clips1;

    [SerializeField] private ParticleSystem[] p_s;

    [SerializeField] private agitar agitar;

    [SerializeField] private GameObject espada;
    [SerializeField] private Rigidbody espada_rb;
    [SerializeField] private BoxCollider espada_bc;
    [SerializeField] private espada_movimiento[] espada_mv;
    [SerializeField] private Transform punto_curva;
    [SerializeField] private Transform posini;
    public float fuerza;
    [SerializeField] private float salto_delay;
    [SerializeField] private GameObject salto_obj;
    [SerializeField] private Collider salto_collider;
    [SerializeField] private float punto_alto=2f;
    [SerializeField] private Light luz_espada;
    [SerializeField] private Camera camara;
    [SerializeField] private Transform posi;
    private Vector3 salto_punto;
    private bool tiene_arma = true;

    private Vector3 pos_jalar;
    private float timer_recuperar=0f;
    private bool jalando = false;
    private Vector3 posoriginal;
    private Vector3 rotoriginal;

    float vol;
    float reparto;

    void Start()
    {
        posoriginal = espada.transform.localPosition;
        rotoriginal = espada.transform.localEulerAngles;
        p_s[0].Stop();
        p_s[1].Stop();
        p_s[2].Stop();
        vol = espada_mv[0].a_s[0].volume;
    }

    void Update()
    {
        if (tiene_arma)
        {
            if (shield.defending == false)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (can_attack)
                    {
                        if (mov.suelo)
                            swordattack();
                        else
                        {
                            swordattack_air();
                        }
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (can_attack)
                    {
                        swordthrow();
                    }
                }
            }
            if (air == 1)
            {
                if (mov.suelo)
                {
                    air = 0;
                    swordattack_fall();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!jalando)
                {
                    sword_start_recover();
                    reparto = vol / 60f;
                    if (!mov.suelo)
                        jump_to_sword();
                }
            }
            if (jalando)
            {
                espada_mv[0].a_s[0].volume -= reparto;
                if (timer_recuperar < 1)
                {
                    espada.transform.position = GetQuadraticCurvePoint(timer_recuperar, pos_jalar, punto_curva.position, posini.position);
                    timer_recuperar += Time.deltaTime * 1.5f;
                }
                else
                {
                    sword_catch();
                    espada_mv[0].a_s[0].volume = vol;
                }
            }
        }
    }

    public void swordattack()
    {
        can_attack = false;
        Animator anim = Sword.GetComponent<Animator>();
        anim.SetBool("attack",true);
        a_s[0].pitch = Random.Range(0.85f, 1.2f);
        a_s[0].PlayOneShot(clips1[0]);
        StartCoroutine(reset_attack_cooldown(0.4f));
        p_s[0].Play(false);
        StartCoroutine(reset_p(0.6f));
    }

    public void swordthrow()
    {
        shield.shielddefend(false);
        can_attack = false;
        Animator anim = Sword.GetComponent<Animator>();
        anim.SetTrigger("throw1");
        a_s[0].pitch = Random.Range(0.5f, 0.6f);
        a_s[0].PlayOneShot(clips1[0]);
        tiene_arma = false;
        p_s[0].Play();
        p_s[1].Play();
        p_s[2].Play();
        StartCoroutine(reset_light(0.4f, true));
    }

    public void swordattack_air()
    {
        can_attack = false;
        Animator anim = Sword.GetComponent<Animator>();
        anim.SetTrigger("attack_air");
        air++;
        mov.caer();
        a_s[0].pitch = Random.Range(0.8f, 0.85f);
        a_s[0].PlayOneShot(clips1[0]);
        p_s[0].Play(false);
    }

    public void swordattack_fall()
    {
        espada_mv[1].damage = 70;
        Animator anim = Sword.GetComponent<Animator>();
        anim.SetTrigger("attack_fall");
        StartCoroutine(reset_attack_cooldown(1.5f));
        a_s[0].pitch = Random.Range(0.90f, 1.0f);
        a_s[0].PlayOneShot(clips1[1]);
        StartCoroutine(reset_p(0.8f));
        agitar.Truama += 0.3f;
    }

    public void sword_start_recover()
    {
        pos_jalar = espada.transform.position;
        espada_rb.isKinematic = false;
        jalando = true;
        espada_mv[0].girando = true;
        espada_mv[0].activada = false;
        StartCoroutine(reset_light(0.5f, false));
        espada_mv[0].atacado = true;
    }

    public void sword_catch()
    {
        espada_rb.interpolation = RigidbodyInterpolation.None;
        agitar.Truama += 0.1f;
        SetLayerRecursively(espada, 18);
        espada_rb.isKinematic = true;
        espada_mv[0].atacado = false;
        espada_mv[0].girando = false;
        espada_mv[0].activada = false;
        espada_rb.transform.parent = posini;
        timer_recuperar = 0f;
        jalando = false;
        espada.transform.localEulerAngles = rotoriginal;
        espada.transform.localPosition = posoriginal;
        tiene_arma = true;
        StartCoroutine(reset_p(0.03f));
        StartCoroutine(reset_attack_cooldown(0.01f));
        espada_mv[0].stop_sound();
        mov.armado = true;
    }

    public void jump_to_sword()
    {
        Vector3 fromPosition = posi.position;
        Vector3 toPosition = espada_bc.bounds.center;
        Vector3 direction = toPosition - fromPosition;

        RaycastHit hit;
        if (Physics.Raycast(posi.position, direction, out hit, 5000f))
        {
            if (hit.transform == salto_obj.transform)
            {
                cam.cambiarfov(105f);
                mov.parabola = true;
                salto_punto = salto_obj.transform.position;
                Invoke(nameof(execute_jump), salto_delay);
            }
        }
    }

    public void execute_jump()
    {
        mov.parabola = false;
        mov.detener = true;
        Vector3 puntobajo = new Vector3(mov.transform.position.x, mov.transform.position.y - 1f, mov.transform.position.z);
        float punto_relativo = salto_punto.y - puntobajo.y;
        float punto_arco = punto_relativo + punto_alto;
        if (punto_relativo < 0) punto_arco = punto_alto;
        StartCoroutine(reset_s(0.1f,punto_arco));
    }

    IEnumerator reset_attack_cooldown(float t)
    {
        yield return new WaitForSeconds(t);
        can_attack = true;
        Animator anim = Sword.GetComponent<Animator>();
        anim.SetBool("attack", false);
        espada_mv[0].damage = 25;
        espada_mv[1].damage = 25;
    }
    IEnumerator reset_p(float t)
    {
        yield return new WaitForSeconds(t);
        p_s[0].Stop();
    }

    IEnumerator reset_light(float t,bool b)
    {
        yield return new WaitForSeconds(t);
        if (b)
            luz_espada.enabled = true;
        else
        {
            luz_espada.enabled = false;
            p_s[1].Stop();
            p_s[2].Stop();
        }
    }

    IEnumerator reset_l(float t)
    {
        yield return new WaitForSeconds(t);
        SetLayerRecursively(espada, 13);
    }

    IEnumerator reset_j(float t)
    {
        yield return new WaitForSeconds(t);
        mov.detener = false;
        cam.cambiarfov(90f);
    }

    IEnumerator reset_s(float t,float punto_arco)
    {
        yield return new WaitForSeconds(t);
        mov.rb.velocity = calcular_velocidad_salto(mov.transform.position, salto_punto, punto_arco);
    }

    public void sword_sound_attack()
    {
        a_s[0].PlayOneShot(clips1[0]);
    }

    public void lanzar()
    {
        espada_rb.transform.parent = null;
        espada_rb.isKinematic = false;
        espada_mv[0].damage = 34;
        Vector3 direccion = cam.transform.forward;
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position,cam.transform.forward,out hit, 500f,10))
        {
            direccion = (hit.point - posini.position).normalized;
        }

        espada_rb.AddForce(direccion * fuerza*1.4f, ForceMode.Impulse);
        StartCoroutine(reset_l(0.0f));
        espada_mv[0].play_sound();
        mov.armado = false;
        espada_mv[0].stop_running();
        espada_rb.interpolation = RigidbodyInterpolation.Interpolate; 
    }

    public Vector3 GetQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }

    public static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }

    public Vector3 calcular_velocidad_salto(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) +
            Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    public void drop_sword()
    {
        espada_rb.transform.parent = null;
        espada_rb.isKinematic = false;
        Vector3 direccion = cam.transform.forward;
        espada_rb.AddForce(direccion * fuerza * 0.7f, ForceMode.Impulse);
        SetLayerRecursively(espada,13);
        Destroy(gameObject.GetComponent<sword1>());
    }
}
