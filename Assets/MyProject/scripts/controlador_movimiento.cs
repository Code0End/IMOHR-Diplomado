using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlador_movimiento : MonoBehaviour
{
    public float velocidad;
    public float altura;
    public float fuerzasalto;
    public float v_aire;
    public float coolsalto;
    public float dash_s;
    public float dash_t;
    public LayerMask piso;
    public float drag_suelo;
    public float deslizar_fuerza;
    private float deslizamiento, tasa_deslizado, tasa_sonido;
    public float deslizar_tiempo;
    [HideInInspector] public bool suelo;
    bool salto_l;
    bool canDash;
    private bool parts = false;
    public bool armado = true;

    public float escalaYagache;
    private float escalaYini;
    private float timer_deslizar;
    [HideInInspector]public bool deslizando;

    public character chara;
    public Transform orientacion;
    public controlador_camara camara;

    [SerializeField] private AudioSource[] a_s;
    [SerializeField] private AudioClip[] clips1;
    [SerializeField] private ParticleSystem[] p_s;
    [SerializeField] private agitar agitar;
    public bool detener = false;
    public bool parabola = false;

    float inputh;
    float inputv;
    public KeyCode tecla_salto = KeyCode.Space;
    public KeyCode tecla_agache = KeyCode.LeftControl;

    Vector3 direc_mov;

    public Rigidbody rb;

    private float dt = 0;
    private int iplus = 0;
    private float iminus = 0;
    private int jumping = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        salto_l = true;
        canDash = true;
        escalaYini = transform.localScale.y;
        deslizamiento = deslizar_fuerza;
        tasa_deslizado = deslizar_fuerza / deslizar_tiempo;
    }

    private void Update()
    {
        //checar si estoy en el suelo
        suelo = Physics.Raycast(transform.position, Vector3.down, altura * 0.5f + 0.2f, piso);

        if (!detener)
        {
            control();
            control_velocidad();
        }

        if (parabola)
            rb.velocity = Vector3.zero;

        //aplicar drag
        if (suelo)
        {
            rb.drag = drag_suelo;
            if (jumping == 1)
            {
                a_s[0].pitch = Random.Range(0.85f, 1.2f);
                a_s[0].PlayOneShot(clips1[0]);
                jumping = 0;
                //detener = false;
                //camara.cambiarfov(90f);
            }
            if (true)
            {
                detener = false;
                camara.cambiarfov(90f);
            }
        }
        else
            rb.drag = 0;

        if (dash_t > 0)
            dash_t -= Time.deltaTime;

        var em = p_s[0].emission;
        if (parts && armado)
            em.enabled = true;
        else
            em.enabled = false;
    }

    private void FixedUpdate()
    {
        if(!detener)
            mover();
        if (deslizando)
            deslizar();
    }

    private void control()
    {
        inputh = Input.GetAxisRaw("Horizontal");
        inputv = Input.GetAxisRaw("Vertical");

        bool abajo = true;
        //salto
        if (Input.GetKey(tecla_salto) && salto_l && suelo)
        {
            salto_l = false;
            salto();
            Invoke(nameof(reiniciar_salto), coolsalto);
        }
        //dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && chara.checkstamina() > 20)
        {
            dash();
        }
        //agache
        if (Input.GetKeyDown(tecla_agache))
        {
            transform.localScale = new Vector3(transform.localScale.x, escalaYagache, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            if (inputh == 0 && inputv == 0)
                parts = false;
            else
            {
                iniciar_delizar();
                parts = true;
            }
        }
        //levantar
        if (Input.GetKeyUp(tecla_agache))
            {
             transform.localScale = new Vector3(transform.localScale.x, escalaYini, transform.localScale.z);
             detener_deslizar();
            }
        if (inputh == 0 && inputv == 0)
            parts = false;
        else
            parts = true;
            }

    private void control_velocidad()
    {
        Vector3 velocidad_n = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        //limitar velocidad
        if(velocidad_n.magnitude > velocidad)
        {
            Vector3 velocidad_l = velocidad_n.normalized * velocidad;
            rb.velocity = new Vector3(velocidad_l.x, rb.velocity.y, velocidad_l.z);
        }
    }

    private void mover()
    {
        if (!deslizando)
        {
            //calcular direccion
            direc_mov = orientacion.forward * inputv + orientacion.right * inputh;

            if (suelo)
                rb.AddForce(direc_mov.normalized * velocidad * 10f, ForceMode.Force);
        }

        //aire
        if (!suelo)
            rb.AddForce(direc_mov.normalized * velocidad * 10f * v_aire, ForceMode.Force);
    }

    private void salto()
    {
        //reiniciar velocidad
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * fuerzasalto, ForceMode.Impulse);
    }

    private void dash()
    {
        if (dash_t > 0) return;
        else dash_t = 0.3f;

        Vector3 movementvect = direc_mov;

        chara.usarstamina(30);

        fuerzacondelay = movementvect.normalized;
        

        Invoke(nameof(dashd), 0.025f);

        Invoke(nameof(dashn),0.3f);
    }

    private Vector3 fuerzacondelay;
    private void dashd()
    {
        rb.AddForce(fuerzacondelay * 200, ForceMode.Force);
        velocidad = dash_s;
        camara.cambiarfov(105);
    }
    private void dashn()
    {
        velocidad = 8;
        camara.cambiarfov(90);
    }

    private void kickd()
    {
        rb.AddForce(camara.transform.up * 700, ForceMode.Force);
        rb.AddForce(-camara.transform.forward * 700, ForceMode.Force);
        camara.cambiarfov(105);
        a_s[0].pitch = Random.Range(0.85f, 1.2f);
        a_s[0].PlayOneShot(clips1[2]);
    }
    private void kickn()
    {
        velocidad = 8;
        camara.cambiarfov(90);
    }

    private void reiniciar_salto()
    {
        salto_l = true;
        jumping = 1;
    }

    private void iniciar_delizar()
    {
        deslizando = true;
        timer_deslizar = deslizar_tiempo;
        a_s[1].pitch = Random.Range(0.95f, 1.2f);
        if (suelo)
        {
            a_s[1].volume = 0.3f;
            var em2 = p_s[1].emission;
            em2.enabled = true;
        }
        else
            a_s[1].volume = 0f;
        a_s[1].PlayOneShot(clips1[1]);
        tasa_sonido = a_s[1].pitch / deslizar_tiempo;
    }

    private void deslizar()
    {      
        Vector3 movementvect = direc_mov;
        var em2 = p_s[1].emission;
        fuerzacondelay = movementvect.normalized;
        RaycastHit hit;

        if (suelo)
        {
            em2.enabled = true;
            a_s[1].volume = 0.3f;
            rb.AddForce(fuerzacondelay * deslizamiento, ForceMode.Force);
            if (inputv == 1)
            {
                if (Physics.Raycast(camara.transform.position, camara.transform.forward, out hit, 2f))
                    if (hit.transform.gameObject.layer == 12)
                    {
                        detener_deslizar();
                        Invoke(nameof(kickd), 0.01f);
                        Invoke(nameof(kickn), 1f);
                    }
            }
        }
        else
        {
            a_s[1].volume = 0f;
            em2.enabled = false;
        }

        deslizamiento -= tasa_deslizado * Time.deltaTime;

        timer_deslizar -= Time.deltaTime;

        a_s[1].pitch -= tasa_sonido * Time.deltaTime;

        if (timer_deslizar <= 0)
            detener_deslizar();
    }

    private void detener_deslizar()
    {
        var em2 = p_s[1].emission;
        em2.enabled = false;
        deslizando = false;
        deslizamiento = deslizar_fuerza;
        a_s[1].Stop();
        a_s[1].volume = 0.3f;
    }

    public void caer()
    {
        rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);
        rb.AddForce(orientacion.forward * 10f, ForceMode.Impulse);
    }
}
