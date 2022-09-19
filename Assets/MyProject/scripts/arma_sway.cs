using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arma_sway : MonoBehaviour
{
    [Header("Configuración de arrastre")]
    public float suave;
    public float multiplicador;
	public controlador_movimiento mov;

	[Header("Configuración de Bob")]
	private const float multiplicador_interno = 6.0f;
	public float multiplicador_frente = 30;
	private float cantidad_bob_frente = 0.0f;
	[Range(0.1f, 10f)] public float multiplicador_bob = 1.0f;
	[Range(0.05f, 0.1f)] public float max_bob_frente = 0.05f;
    [HideInInspector] public float velocidad_bob = 1.0f;
    [Range(0.02f, 0.3f)] public float velocidad_bob_lado = 0.15f;
    [Range(0.02f, 0.04f)] public float cantidad_bob_lado = 0.02f;
	int counter = 0;

	private Vector3 Posini;
	private float timer = 0;

	float aHorz;
	float aVert;

	bool sueloa = false;
	private void Update()
    {

        rotar();
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		aHorz = Mathf.Abs(horizontal);
		aVert = Mathf.Abs(vertical);
        if (mov.deslizando)
        {
			sueloa = true;
			aHorz = 0;
			aVert = 0;
			bob();
		}
		if (mov.suelo)
		{
			if(sueloa == true)
            {
				aHorz = 1;
				aVert = 1;
				counter++;
				if (counter >= 40)
					counter = 0;
			}
			bob();
			if(counter == 0)
				sueloa = false;
		}
		else
		{
			sueloa = true;
			aHorz = 0;
			aVert = 0;
			bob();
		}
    }
    
    private void rotar()
    {
        //obtener input del mouse
        float mouseX = Input.GetAxisRaw("Mouse X") * multiplicador;
        float mouseY = Input.GetAxisRaw("Mouse Y") * multiplicador;

        //calcular rotación
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion rotacionObjetivo = rotationX * rotationY;

        //rotar el arma
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotacionObjetivo, suave * Time.deltaTime);
    }

    private void bob()
    {
		bool movimientoX=false;
		bool movimientoY=false;
		if (sueloa)
		{
			movimientoX = true;
			movimientoY = true;
		}
		else
		{
			movimientoX = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S);
		    movimientoY = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
		}

		if ((movimientoX && aVert > 0) || (movimientoY && aHorz > 0))
		{
			float velocidadmovimiento = Mathf.Clamp(aVert + aHorz, 0, 1) * velocidad_bob * multiplicador_frente;

			cantidad_bob_frente += velocidadmovimiento;
		}
		else
		{
			cantidad_bob_frente -= velocidad_bob * multiplicador_frente;

			if (cantidad_bob_frente < 0)
			{
				cantidad_bob_frente = 0;
			}
		}

		cantidad_bob_frente *= Time.deltaTime;

		float movx = 0.0f;
		float movy = 0.0f;

		Vector3 posicioncalculada = Posini;

		if (aHorz == 0 && aVert == 0)
		{
			timer = 0.0f;
		}
		else
		{
			movx = Mathf.Sin(timer);
			movy = -Mathf.Abs(Mathf.Abs(movx) - 1);

			timer += velocidad_bob_lado;

			if (timer > Mathf.PI * 2)
			{
				timer = timer - (Mathf.PI * 2);
			}
		}

		float movimientototal = Mathf.Clamp(aVert + aHorz, 0, 1);

		if (movx != 0)
		{
			movx = movx * movimientototal;
			posicioncalculada.x = Posini.x + movx * cantidad_bob_lado;
		}
		else
		{
			posicioncalculada.x = Posini.x;
		}

		if (movy != 0)
		{
			movy = movy * movimientototal;
			posicioncalculada.y = Posini.y + movy * cantidad_bob_lado * 2;
		}
		else
		{
			posicioncalculada.y = Posini.y;
		}

		float totalFrontX = Mathf.Clamp(cantidad_bob_frente, -max_bob_frente, max_bob_frente);
		float totalFrontY = Mathf.Clamp(cantidad_bob_frente, -max_bob_frente, max_bob_frente);
		float totalFrontZ = Mathf.Clamp(cantidad_bob_frente, -max_bob_frente, max_bob_frente);

		posicioncalculada.x += totalFrontX;
		posicioncalculada.y -= totalFrontY;
		posicioncalculada.z -= totalFrontZ;

		transform.localPosition = Vector3.Lerp(transform.localPosition, posicioncalculada, Time.deltaTime * multiplicador_interno * multiplicador_bob);
	
	}
}
