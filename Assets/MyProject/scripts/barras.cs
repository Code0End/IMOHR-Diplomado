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
        barra_hp.fillAmount = parte;
    }
    public void updatestamina(float parte)
    {
        barra_stamina.fillAmount = parte;
    }
}
