using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.Rendering.HighDefinition;

public class enemy_comportamiento : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform jugador;

    public LayerMask suelo, Ljugador;

    [SerializeField] private AudioSource[] a_s;
    [SerializeField] private AudioClip[] clips1;

    //Patrullando
    public Vector3 walkPoint;
    bool walkPointSet,wps2, dpatrullar,a,p,r;
    public bool death;
    public float walkPointRange;
    int patrullaje = 0,ataque=0;
    float ar = 2f;

    //Estados
    public float rangovista, rangoataque;
    public bool rangojugador, rangoataquej;

    private string currentState;

    const string idle = "Giant@Idle01";
    const string walk = "walkf";
    const string run = "Giant@Run01 - Forward";
    const string search = "Giant@Idle01a";
    const string punch = "Giant@UnarmedAttack02";
    const string magic1 = "Giant@Win01 - Start";
    const string magic2 = "Giant@Spawn01";

    //ataques
    public Transform[] pro;
    public GameObject[] ptil;

    public Animator anime;
    public Light l;
    public ParticleSystem ps;

    HDAdditionalLightData HDL;


    float danterior,f=2,f2;




    private void Awake()
    {
        jugador = GameObject.Find("Jugador").transform;
        agent = GetComponent<NavMeshAgent>();
        HDL = l.GetComponent<HDAdditionalLightData>();
        ps.Stop();
    }

    private void Update()
    {
        //if (death) this.enabled = false;
        //Revisar vision y rango de ataque
        rangojugador = Physics.CheckSphere(transform.position, rangovista, Ljugador);
        rangoataquej = Physics.CheckSphere(transform.position, rangoataque, Ljugador);


        HDL.intensity = f;


        if (!rangojugador && !rangoataquej) Patrullar();
        if (rangojugador && !rangoataquej) Perseguir();
        if (rangoataquej && rangojugador) Atacar();

    }

    private void Patrullar()
    {
        p = false;
        if (!dpatrullar)
            decidir_patrullar();

        if (patrullaje == 0)
        {
            if (!walkPointSet)
            {
                walkPoint = RandomNavmeshLocation(walkPointRange);
                agent.SetDestination(walkPoint);
            }

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            if (distanceToWalkPoint.magnitude == danterior)
                walkPointSet = false;

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 0.4f)
            {
                walkPointSet = false;
                ChangeAnimationState(idle);
                patrullaje = 3;
                StartCoroutine(reset_p(Random.Range(1f, 2f)));
            }
            danterior = distanceToWalkPoint.magnitude;
        }

        if (patrullaje == 1)
        {
            ChangeAnimationState(search);
        }

        if (patrullaje == 2)
        {
            ChangeAnimationState(idle);
        }


    }

    private void decidir_patrullar()
    {
        dpatrullar = true;
        int r = UnityEngine.Random.Range(1, 3);
        switch (r) {
            case 1:
                patrullaje = 0;
                ChangeAnimationState(walk);
                break;
            case 2:
                patrullaje = 1;
                StartCoroutine(reset_p(Random.Range(2f, 4f)));
                break;
            case 3:
                patrullaje = 2;
                StartCoroutine(reset_p(Random.Range(1f, 5f)));
                break;
        }
    }

    IEnumerator reset_p(float t)
    {
        yield return new WaitForSeconds(t);
        dpatrullar = false;
    }

    private void Perseguir()
    {
        r = true;
        if (!p)
        {
            p = true;
            a_s[0].volume = 0.6f;
            a_s[0].pitch = Random.Range(0.80f, 1f);
            a_s[0].PlayOneShot(clips1[0]);
        }


        rangoataque = 10;
        agent.SetDestination(jugador.position);
        ChangeAnimationState(walk);
    }

    private void Atacar()
    {
        p = false;

        if (r)
        {
            ataque = Random.RandomRange(1, 10);
            r = false;
        }

        if (ataque >= 6)
        {
            rangoataque = 20;
            //Enemigo se queda quieto cuando ataca
            agent.SetDestination(transform.position);
            Vector3 targetpos = new Vector3(jugador.transform.position.x, transform.position.y, jugador.transform.position.z);
            transform.LookAt(targetpos);

            
            if (a) return;
                a = true;

            
            DOTween.To(() => f, x => f = x, 10, 0.5f);
            StartCoroutine(reset_l(1f));
            ps.Play();

            a_s[0].volume = 0.6f;
            a_s[0].pitch = Random.Range(0.85f, 1.2f);
            a_s[0].PlayOneShot(clips1[2]);


            
            ChangeAnimationState(magic2);;
            Invoke(nameof(objectSpawn), 1.25f);
            StartCoroutine(reset_i(Random.Range(1.50f,2.5f)));
            
        }
        else if (ataque >= 3 && ataque <= 5)
        {
            rangoataque = 30;
            //Enemigo se queda quieto cuando ataca
            agent.SetDestination(transform.position);

            Vector3 targetpos = new Vector3(jugador.transform.position.x, transform.position.y, jugador.transform.position.z);
            transform.LookAt(targetpos);

            if (a) return;
            a = true;


            DOTween.To(() => f, x => f = x, 10, 0.5f);
            StartCoroutine(reset_l(1f));
            ps.Play();

            a_s[0].volume = 0.6f;
            a_s[0].pitch = Random.Range(0.85f, 1.2f);
            a_s[0].PlayOneShot(clips1[2]);

            ChangeAnimationState(magic1);
            Invoke(nameof(objectSpawnp), 1.25f);
            StartCoroutine(reset_i(Random.Range(2f, 2.7f)));

        }
        else if (ataque < 3)
        {
            Vector3 direction = jugador.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction);

            if (!wps2)
            {
                if (!a)
                {
                    agent.SetDestination(jugador.position);
                    ChangeAnimationState(walk);
                }
            }

            Vector3 distanceToWalkPoint = transform.position - jugador.position;

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < ar)
            {
                ar = 5f;
                agent.SetDestination(transform.position);
                wps2 = true;
                if (a)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 2.5f * Time.deltaTime);
                    return;
                }
                a = true;
                DOTween.To(() => f, x => f = x, 10, 0.5f);
                StartCoroutine(reset_l(1f));
                ps.Play();

                a_s[0].volume = 0.6f;
                a_s[0].pitch = Random.Range(0.85f, 1.2f);
                a_s[0].PlayOneShot(clips1[2]);

                ChangeAnimationState(punch);
                Invoke(nameof(objectSpawna), 0.7f);
                StartCoroutine(reset_i(Random.Range(2f, 3.1f)));
            }
            else
            {
                ar = 2f;
                wps2 = false;
            }
        }   
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        anime.CrossFade(newState, 0.1f);
        currentState = newState;
    }

    IEnumerator reset_i(float t)
    {
        yield return new WaitForSeconds(t);
        a = false;
        if(ataque < 3)
        {
            var i = Random.RandomRange(1, 6); 
            if(i <= 3)
                r = true;
        }
        else
        {
            r = true;
        }
    }

    IEnumerator reset_l(float t)
    {
        yield return new WaitForSeconds(t);
        DOTween.To(() => f, x => f = x, 2, 0.5f);
        ps.Stop();
    }

    public Vector3 RandomNavmeshLocation(float radius) {
         Vector3 randomDirection = Random.insideUnitSphere * radius;
         randomDirection += transform.position;
         NavMeshHit hit;
         Vector3 finalPosition = Vector3.zero;
         if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
             finalPosition = hit.position;            
         }
         walkPointSet = true;
         return finalPosition;
     }

    void objectSpawn()
    {
        Transform instance = Instantiate(ptil[0], pro[0].transform).transform;
        instance.parent = null;
        Vector3 todir = jugador.position - pro[0].transform.position;
        Vector3 newdir = Vector3.RotateTowards(instance.forward, todir,360f,500f);
        instance.rotation = Quaternion.LookRotation(newdir);
        ChangeAnimationState(idle);
    }

    void objectSpawnp()
    {
        Transform instance = Instantiate(ptil[1], pro[1].transform).transform;
        instance.parent = null;

        Vector3 puntobajo = new Vector3(instance.position.x, instance.position.y - 1f, instance.position.z);
        float punto_relativo = jugador.transform.position.y - puntobajo.y;
        float punto_arco = punto_relativo + 2f;
        if (punto_relativo < 0) punto_arco = 2f;

        instance.gameObject.GetComponent<Rigidbody>().velocity = calcular_velocidad_salto(instance.position, jugador.transform.position, punto_arco);

        ChangeAnimationState(idle);
    }

    void objectSpawna()
    {
        Transform instance = Instantiate(ptil[2], pro[2].transform).transform;
        instance.parent = null;

        StartCoroutine(reset_a(1f));
    }

    IEnumerator reset_a(float t)
    {
        yield return new WaitForSeconds(t);
        ChangeAnimationState(idle);
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

}

