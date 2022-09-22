using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class controlador_camara : MonoBehaviour
{
    [SerializeField] private float sens;
    [SerializeField] private controlador_movimiento controlador;
    float rotx;
    float roty;

    public float tilta;
    public bool d;
    float iv = 0f;
    float iv2 = 0f;
    float iv3 = 0f;

    [SerializeField] Transform orient;

    //smoothing de la cámara
    private static int m_maxrotcache = 3;
    private float[] m_rotarrayx = new float[m_maxrotcache];
    private float[] m_rotarrayy = new float[m_maxrotcache];
    private int m_rotcacheindex = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mousex = Input.GetAxisRaw("Mouse X") * sens;
        float mousey = Input.GetAxisRaw("Mouse Y") * sens;

        if (!d)
        {
            iv = -Input.GetAxis("Horizontal") * tilta;
            iv3 = Mathf.Lerp(iv, iv2, 0.5f);
            iv2 = iv;
        }

        roty += mousex;
        rotx -= mousey;
        rotx = Mathf.Clamp(rotx, -90f, 90f);

        rotx = Obtener_promedio_rx(rotx);
        roty = Obtener_promedio_ry(roty);
        incrementar_rotcacheindex();

        transform.rotation = Quaternion.Euler(rotx, roty, iv3);
        orient.rotation = Quaternion.Euler(0, roty, 0);
    }

    private float Obtener_promedio_rx(float rx)
    {
        m_rotarrayx[m_rotcacheindex] = rx;
        float result = 0f;
        for (int i = 0; i < m_rotarrayx.Length; i++)
        {
            result += m_rotarrayx[i];
        }
        return result / m_rotarrayx.Length;
    }

    private float Obtener_promedio_ry(float ry)
    {
        m_rotarrayy[m_rotcacheindex] = ry;
        float result = 0f;
        for (int i = 0; i < m_rotarrayy.Length; i++)
        {
            result += m_rotarrayy[i];
        }
        return result / m_rotarrayy.Length;
    }

    private void incrementar_rotcacheindex()
    {
        m_rotcacheindex++;
        m_rotcacheindex %= m_maxrotcache;
    }

    public void cambiarfov(float valor)
    {
        GetComponent<Camera>().DOFieldOfView(valor, 0.25f);
    }

    }