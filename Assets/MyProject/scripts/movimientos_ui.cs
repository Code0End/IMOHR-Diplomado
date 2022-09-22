using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimientos_ui : MonoBehaviour
{
    public int estado = 0;
    public bool destruir = false;
    private void OnEnable()
    {
        transform.localScale = new Vector3(0, 0, 0);
        entrada();
    }

    public void entrada()
    {
        LeanTween.rotate(gameObject, Vector3.zero, 0f);
        if (estado == 0)
            LeanTween.scale(gameObject, new Vector3(1, 1, 1), 1f).setDelay(0.3f).setEase(LeanTweenType.easeInOutSine);
        if (estado == 1)
            LeanTween.scale(gameObject, new Vector3(1, 1, 1), 1f).setDelay(0.3f).setEase(LeanTweenType.easeInOutSine).setOnComplete(mov_1);
        if (estado == 2)
        {
            LeanTween.scale(gameObject, new Vector3(1, 1, 1), 1f).setDelay(0.3f).setEase(LeanTweenType.easeInOutSine).setOnComplete(mov_1);
            rot_1();
        }
    }

    public void salida()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 1f).setEase(LeanTweenType.easeInOutSine).setOnComplete(salida_d);
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
        LeanTween.scale(gameObject, new Vector3(tamaño_random, tamaño_random, tamaño_random), 1f).setEase(LeanTweenType.easeInOutSine).setOnComplete(mov_2);
    }
    public void mov_2()
    {
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 1f).setEase(LeanTweenType.easeInOutSine).setOnComplete(mov_1);
    }

    public void rot_1()
    {
        float tiempo_random, rotacion_random;
        tiempo_random = Random.Range(0.8f, 1.5f);
        rotacion_random = Random.Range(-5f, 5f);
        LeanTween.rotate(gameObject, new Vector3(0, 0, rotacion_random), tiempo_random).setEase(LeanTweenType.easeInOutSine).setOnComplete(rot_2);
    }

    public void rot_2()
    {
        float tiempo_random;
        tiempo_random = Random.Range(0.8f, 1.5f);
        LeanTween.rotate(gameObject, Vector3.zero, tiempo_random).setEase(LeanTweenType.easeInOutSine).setOnComplete(rot_1);
    }

}
