using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class controlador_menu : MonoBehaviour
{

    [SerializeField] private GameObject[] images;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private controlador_movimiento cm;
    [SerializeField] private bob_camara bc;
    [SerializeField] private character ch;
    [SerializeField] private controlador_camara cc;
    [SerializeField] private GameObject[] weapon_holders;
    [SerializeField] private camerarotate cr;
    [SerializeField] private manejador_gameplay mg;
    public bool timer,post;
    public bool game_started;


    void Awake()
    {
        if (!post)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 1F;
        }
    }

    public void game_drop()
    {
        rb.useGravity = true;
        cr.enabled = true;
        Invoke(nameof(game_start), 2.4f);
    }

    public void game_start()
    {
        game_started = true;
        cr.enabled = false;
        images[0].SetActive(true);
        images[1].SetActive(true);
        images[2].SetActive(true);
        images[3].SetActive(true);

        if(timer)
        images[4].SetActive(true);

        cm.enabled = true;
        bc.enabled = true;
        ch.enabled = true;
        cc.enabled = true;
        mg.enabled = true;
        weapon_holders[0].SetActive(true);
        weapon_holders[1].SetActive(true);
        Destroy(gameObject);
    }

    public void timerturn(bool tt)
    {
        if (tt)
            images[4].SetActive(true);
        else
        {
            images[4].GetComponent<movimientos_ui>().StopAllCoroutines();
            images[4].GetComponent<movimientos_ui>().salida();
        }
    }
}
