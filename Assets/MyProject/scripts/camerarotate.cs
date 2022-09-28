using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerarotate : MonoBehaviour
{
    [SerializeField] private float sens = 250f;
    [SerializeField] private float damping = 10;
    float rotx;
    float roty;
    float mousex;
    float mousey;
    [SerializeField] Transform orient;
    public bool post;

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mousex = 0;
        mousey = 0;
    }

    private void Start()
    {
        if (post)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            mousex = 0;
            mousey = 0;
        }
    }

    void Update()
    {
        

        roty += mousex;
        rotx -= mousey;
        rotx = Mathf.Clamp(rotx, -90f, 90f);

        Quaternion Q = Quaternion.Euler(rotx, roty, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Q, Time.deltaTime/0.8f);
        //orient.rotation = Quaternion.Euler(0, roty, 0);
    }

    public void ddd()
    {
        Destroy(this);
    }
}
