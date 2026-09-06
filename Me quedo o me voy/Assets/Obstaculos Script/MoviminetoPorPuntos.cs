using System.Collections.Generic;
using UnityEngine;

public class MovimientoPorPuntos : MonoBehaviour
{
    public Transform puntoA;
    
    public Transform puntoB;
    
    public float speed = 2f;

    [Header("Empujón/Ragdoll al tocar al jugador")]
    public float fuerzaDeEmpuje = 15f;
    public float fuerzaVertical = 4f;
    public float cooldownEntreGolpes = 0.5f;

    private Dictionary<Rigidbody, float> ultimoGolpe = new Dictionary<Rigidbody, float>();

    private Transform objetivoActual;

    void Start()
    {
        objetivoActual = puntoB;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, objetivoActual.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, objetivoActual.position) < 0.1f)
        {
            if (objetivoActual == puntoA)
            {
                objetivoActual = puntoB;
            }
            else
            {
                objetivoActual = puntoA;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        controlar jugador = collision.gameObject.GetComponent<controlar>();
        if (jugador == null) return;

        if (!jugador.IsOwner) return;

        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb == null) return;

        if (ultimoGolpe.TryGetValue(rb, out float ultimaVez))
        {
            if (Time.time - ultimaVez < cooldownEntreGolpes) return;
        }
        ultimoGolpe[rb] = Time.time;

        Vector3 direccionEmpuje = (collision.transform.position - transform.position).normalized;
        direccionEmpuje += Vector3.up * (fuerzaVertical / Mathf.Max(fuerzaDeEmpuje, 0.01f));
        direccionEmpuje.Normalize();

        Ragdoll ragdoll = collision.gameObject.GetComponent<Ragdoll>();
        if (ragdoll != null)
        {
            ragdoll.ActivarRagdoll(direccionEmpuje * fuerzaDeEmpuje);
        }
        else
        {
            rb.AddForce(direccionEmpuje * fuerzaDeEmpuje, ForceMode.Impulse);
        }
    }
}