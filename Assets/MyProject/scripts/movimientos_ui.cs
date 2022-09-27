using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class movimientos_ui : MonoBehaviour
{
    public int estado = 0;
    public bool destruir = false;

    public GameObject padrecam;

    [SerializeField] controlador_menu c_m;

    [SerializeField] private AudioSource[] a_s;
    [SerializeField] private AudioClip[] clips1;

    //agitar cámara
    public bool agitar_activo = true;
    [Range(0, 1)][SerializeField] float trauma;
    [SerializeField] float traumam = 5f;
    [SerializeField] float traumamag = 0.8f;
    [SerializeField] float traumarotmag = 1.5f;
    [SerializeField] float traumaprofmag = 1.3f;
    [SerializeField] float traumadis = 1.3f;
    float timer;

    public List<GameObject> elementos_apagar;
    public List<GameObject> elementos_encender;

    public Volume volumen;
    public movimientos_ui mui1;
    private ColorAdjustments s;
    bool e,bp,paused;

    public float f3 = 1f, f4 = 1f, f5 = 1f;

    void Update()
    {
        if (e)
        {
            Color color = new Vector4(f3, f4, f5, 1.0f);
            s.colorFilter.value = color;
        }
        if (agitar_activo && Truama > 0)
        {
            timer += Time.unscaledDeltaTime * Mathf.Pow(trauma, 0.3f) * traumam;
            Vector3 newpos = obtener_vec3() * traumamag * Truama;
            transform.localPosition = newpos;
            transform.localRotation = Quaternion.Euler(newpos * traumarotmag);
            Truama -= Time.unscaledDeltaTime * traumadis * Truama;
        }
        else
        {
            Vector3 newpos = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.unscaledDeltaTime);
            transform.localPosition = newpos;
            transform.localRotation = Quaternion.Euler(newpos * traumarotmag);
        }        
    }


    private void OnEnable()
    {
        transform.localScale = new Vector3(0, 0, 0);
        entrada();
    }

    public void entrada()
    {
        LeanTween.rotate(gameObject, Vector3.zero, 0f).setIgnoreTimeScale(true);
        if (estado == 0)
            LeanTween.scale(gameObject, new Vector3(1, 1, 1), 1f).setDelay(0.3f).setEase(LeanTweenType.easeInOutSine).setIgnoreTimeScale(true);
        if (estado == 1)
            LeanTween.scale(gameObject, new Vector3(1, 1, 1), 1f).setDelay(0.3f).setEase(LeanTweenType.easeInOutSine).setOnComplete(mov_1).setIgnoreTimeScale(true);
        if (estado == 2)
        {
            LeanTween.scale(gameObject, new Vector3(1, 1, 1), 1f).setDelay(0.3f).setEase(LeanTweenType.easeInOutSine).setOnComplete(mov_1).setIgnoreTimeScale(true);
            rot_1();
        }
    }

    public void salida()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 1f).setEase(LeanTweenType.easeInOutSine).setOnComplete(salida_d).setIgnoreTimeScale(true);
    }

    public void salida_d()
    {
        if (destruir == true)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void mov_1()
    {
        float tamaño_random;
        tamaño_random = Random.Range(0.85f, 1.15f);
        LeanTween.scale(gameObject, new Vector3(tamaño_random, tamaño_random, tamaño_random), 1f).setEase(LeanTweenType.easeInOutSine).setOnComplete(mov_2).setIgnoreTimeScale(true);
    }
    public void mov_2()
    {
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 1f).setEase(LeanTweenType.easeInOutSine).setOnComplete(mov_1).setIgnoreTimeScale(true);
    }

    public void rot_1()
    {
        float tiempo_random, rotacion_random;
        tiempo_random = Random.Range(0.8f, 1.5f);
        rotacion_random = Random.Range(-5f, 5f);
        LeanTween.rotate(gameObject, new Vector3(0, 0, rotacion_random), tiempo_random).setEase(LeanTweenType.easeInOutSine).setOnComplete(rot_2).setIgnoreTimeScale(true);
    }

    public void rot_2()
    {
        float tiempo_random;
        tiempo_random = Random.Range(0.8f, 1.5f);
        LeanTween.rotate(gameObject, Vector3.zero, tiempo_random).setEase(LeanTweenType.easeInOutSine).setOnComplete(rot_1).setIgnoreTimeScale(true);
    }

    public void click()
    {
        if (bp) return;
        bp = true;
        StartCoroutine(gs4(0.0f, true));
        Truama += 0.8f;
        a_s[0].pitch = Random.Range(0.85f, 1.2f);
        a_s[0].PlayOneShot(clips1[0]);
        Invoke(nameof(gs), 1f);
        //StartCoroutine(gs4(1.1f, false));
    }

    public void click_begin()
    {
        if (bp) return;
        bp = true;
        StartCoroutine(gs4(0.0f, true));
        volumen.profile.TryGet<ColorAdjustments>(out s);
        DOTween.To(() => f3, x => f3 = x, 0f, 5f).SetUpdate(true);
        DOTween.To(() => f4, x => f4 = x, 0f, 5f).SetUpdate(true);
        DOTween.To(() => f5, x => f5 = x, 0f, 5f).SetUpdate(true);
        e = true;
        Truama += 0.8f;
        a_s[0].pitch = Random.Range(0.85f, 1.2f);
        a_s[0].PlayOneShot(clips1[0]);
        Invoke(nameof(gs5), 0f);
    }

    public void click_exit()
    {
        if (bp) return;
        bp = true;
        StartCoroutine(gs4(0.0f, true));
        volumen.profile.TryGet<ColorAdjustments>(out s);
        DOTween.To(() => f3, x => f3 = x, 0f, 5f).SetUpdate(true);
        DOTween.To(() => f4, x => f4 = x, 0f, 5f).SetUpdate(true);
        DOTween.To(() => f5, x => f5 = x, 0f, 5f).SetUpdate(true);
        e = true;
        Truama += 0.8f;
        a_s[0].pitch = Random.Range(0.85f, 1.2f);
        a_s[0].PlayOneShot(clips1[0]);
        Invoke(nameof(gs2), 5f);
    }

    public void click_credits()
    {
        if (bp) return;
        bp = true;
        StartCoroutine(gs4(0.0f, true));
        volumen.profile.TryGet<ColorAdjustments>(out s);
        DOTween.To(() => f3, x => f3 = x, 0f, 5f).SetUpdate(true);
        DOTween.To(() => f4, x => f4 = x, 0f, 5f).SetUpdate(true);
        DOTween.To(() => f5, x => f5 = x, 0f, 5f).SetUpdate(true);
        Truama += 0.8f;
        a_s[0].pitch = Random.Range(0.85f, 1.2f);
        a_s[0].PlayOneShot(clips1[0]);
        Invoke(nameof(gs3), 0f);       
    }

    public void click_options()
    {
        f3 = 1;
        f4 = 1;
        f5 = 1;
        if (bp) return;
        StartCoroutine(gs4(0.0f, true));
        bp = true;
        e = true;
        volumen.profile.TryGet<ColorAdjustments>(out s);
        DOTween.To(() => f3, x => f3 = x, 0.5f, 1f).SetUpdate(true);
        DOTween.To(() => f4, x => f4 = x, 0.5f, 1f).SetUpdate(true);
        DOTween.To(() => f5, x => f5 = x, 0.5f, 1f).SetUpdate(true);
        Truama += 0.8f;
        a_s[0].pitch = Random.Range(0.85f, 1.2f);
        a_s[0].PlayOneShot(clips1[0]);
        Invoke(nameof(gs3), 1.1f);
    }

    public void click_pause()
    {
        if (paused) return;
        paused = true;
        f3 = 1;
        f4 = 1;
        f5 = 1;
        if (bp) return;
        gs4(0f,true);
        bp = true;
        e = true;
        volumen.profile.TryGet<ColorAdjustments>(out s);
        DOTween.To(() => s.colorFilter.value, x => s.colorFilter.value = x, new Vector3(0.5f, 0.5f, 0.5f), 0.2f).SetUpdate(true);
        //DOTween.To(() => f4, x => f4 = x, 0.5f, 0.2f);
        //DOTween.To(() => f5, x => f5 = x, 0.5f, 0.2f);
        Truama += 0.8f;
        a_s[0].pitch = Random.Range(0.4f, 0.6f);
        a_s[0].PlayOneShot(clips1[0]);
        gs3();
        Invoke(nameof(Pausar), 0f);
    }

    public void click_closeoptions()
    {
        f3 = 0.5f;
        f4 = 0.5f;
        f5 = 0.5f;
        if (bp) return;
        bp = true;
        e = true;
        volumen.profile.TryGet<ColorAdjustments>(out s);
        DOTween.To(() => f3, x => f3 = x, 1f, 1f).SetUpdate(true);
        DOTween.To(() => f4, x => f4 = x, 1f, 1f).SetUpdate(true);
        DOTween.To(() => f5, x => f5 = x, 1f, 1f).SetUpdate(true);
        Truama += 0.8f;
        a_s[0].pitch = Random.Range(0.85f, 1.2f);
        a_s[0].PlayOneShot(clips1[0]);
        Invoke(nameof(gs3), 1.1f);
    }

    public void click_closepause()
    {
        continuar();
        mui1.paused = false;
        f3 = 0.5f;
        f4 = 0.5f;
        f5 = 0.5f;
        if (bp) return;
        bp = true;
        e = true;
        volumen.profile.TryGet<ColorAdjustments>(out s);
        DOTween.To(() => s.colorFilter.value, x => s.colorFilter.value = x, new Vector3(1f, 1f,1f), 0.2f).SetUpdate(true);
        //DOTween.To(() => f4, x => f4 = x, 1f, 1f).SetUpdate(true);
        //DOTween.To(() => f5, x => f5 = x, 1f, 1f).SetUpdate(true);
        Truama += 0.8f;
        a_s[0].pitch = Random.Range(0.85f, 1.2f);
        a_s[0].PlayOneShot(clips1[0]);
        gs3(); ;
    }

    public void gs()
    {
        bp = false;
        c_m.game_drop();
    }

    public void gs2()
    {
        bp = false;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void gs5()
    {
        bp = false;
        SceneManager.LoadScene(0);
    }

    public void gs3()
    {
        bp = false;
        foreach (GameObject elemento in elementos_apagar)
        {
            elemento.GetComponent<movimientos_ui>().StopAllCoroutines();
            elemento.GetComponent<movimientos_ui>().salida();
        }

        foreach (GameObject elemento in elementos_encender)
        {
            elemento.SetActive(true);
            elemento.GetComponent<movimientos_ui>().bp = false;
        }
        e = false;
    }

    IEnumerator gs4(float t,bool vb)
    {
        yield return new WaitForSeconds(t);
        bp = false;
        if (vb)
        {
            foreach (GameObject elemento in elementos_apagar)
            {
                elemento.GetComponent<movimientos_ui>().bp = true;
            }
        }
        else
        {
            foreach (GameObject elemento in elementos_encender)
            {
                elemento.GetComponent<movimientos_ui>().bp = false;
            }
        }   
    }

    public float Truama
    {
        get { return trauma; }

        set { trauma = Mathf.Clamp01(value); }
    }

    float obtener_float(float semilla)
    {
        return (Mathf.PerlinNoise(semilla, timer) - 0.5f) * 2;
    }

    Vector3 obtener_vec3()
    {
        return new Vector3(obtener_float(1), obtener_float(10), obtener_float(100) * traumaprofmag);
    }

    void continuar()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Pausar()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }
}
