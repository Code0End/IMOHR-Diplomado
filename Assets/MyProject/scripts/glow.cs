using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using DG.Tweening;
using UnityEngine.UI;

public class glow : MonoBehaviour
{

    public Material mat;
    public Renderer r;
    public GameObject m_EmissiveObject;
    float emissiveIntensity = 2f;
    Color emissiveColor = new Vector4 (0.2823529412f, 0f, 0.4352941176f, 1.0f);

    void Awake()
    {
        r = m_EmissiveObject.GetComponent<Renderer>();

        u_g();
    }

    void Update()
    {
        r.material.SetColor("_EmissiveColor", new Vector4(0.2823529412f, 0f, 0.4352941176f, 1.0f) * Mathf.Pow(2.0f,emissiveIntensity));
        //Debug.Log(r.material.color);
    }

    public void u_g()
    {
        DOTween.To(() => emissiveIntensity, x => emissiveIntensity = x, 10f, 6f);
        Invoke(nameof(d_g), 6.01f);
    }

    public void d_g()
    {
        DOTween.To(() => emissiveIntensity, x => emissiveIntensity = x, 6f, 6f);
        Invoke(nameof(u_g), 6.01f);
    }

    public void self_destroy()
    {
        Destroy(gameObject);
    }


}
