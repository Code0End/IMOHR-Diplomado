using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class manejador_gameplay : MonoBehaviour
{
    [SerializeField] private AudioSource[] a_s;
    [SerializeField] private AudioClip[] clips1;
    [SerializeField] private GameObject[] ts;


    public TMP_Text[] texto;
    public float segundos = 0;
    public bool aumentando = false;
    public bool on = false;

    public void OnEnable()
    {
        on = true;
    }

    void Update()
    {
        if (!on) return;
        if (aumentando == false && on == true)
        {
            aumentando = true;
            segundos += Time.deltaTime;
            float minutos = Mathf.FloorToInt(segundos / 60);
            float displaysegundos = Mathf.FloorToInt(segundos % 60);
            texto[0].text = (minutos.ToString() + ":" + displaysegundos.ToString());
            aumentando = false;
        }        
    }

    public void gameend()
    {
        ts[0].GetComponent<movimientos_ui>().salida();
        ts[1].SetActive(true);
        float minutos = Mathf.FloorToInt(segundos / 60);
        float displaysegundos = Mathf.FloorToInt(segundos % 60);
        texto[0].text = (minutos.ToString() + ":" + displaysegundos.ToString());
        texto[1].text = texto[0].text;
    }
}
