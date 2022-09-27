using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class opciones : MonoBehaviour
{
    public AudioMixer am;
    public controlador_camara cc;
    public float sens;
    public bool pc=true, ti;
    public controlador_menu cm;

    public void cambiar_vol(float v) 
    {
        am.SetFloat("volume_master",v);
    }

    public void cambiar_sens(float v)
    {
        cc.sens = v;
    }

    public void pantalla_completa(bool b)
    {
        Screen.fullScreen = b;
    }

    public void timer_onscreen(bool b)
    {
        cm.timer = b;
    }
}
