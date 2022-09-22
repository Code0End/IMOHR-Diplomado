using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barras : MonoBehaviour
{
    public Image barra_hp;
    public Image barra_stamina;

    public void updatehp(float parte)
    {
        if (barra_hp == null) return;
        barra_hp.fillAmount = parte;
    }
    public void updatestamina(float parte)
    {
        if (barra_stamina == null) return;
        barra_stamina.fillAmount = parte;
    }
}
