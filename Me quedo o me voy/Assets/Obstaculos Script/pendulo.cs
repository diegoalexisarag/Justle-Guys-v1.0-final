using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pendulo : MonoBehaviour
{
    public float speed = 1.5f;
    public float limit = 75f;
    public bool randomStart = false;
    public float fuerzaDeEmpuje = 15f;
    public float fuerzaVertical = 4f;
    public float cooldownEntreGolpes = 0.5f;

    private Dictionary<Rigidbody, float> ultimoGolpe = new Dictionary<Rigidbody, float>();
    
    private float phaseOffset = 0f;
    private float startZRotation; 
    private float startYRotation;
    private float startXRotation;

    void Awake()
    {
        startZRotation = transform.localEulerAngles.z;
        startYRotation = transform.localEulerAngles.y;
        startXRotation = transform.localEulerAngles.x;

        if (randomStart)
        {
            phaseOffset = Random.Range(0f, Mathf.PI * 2f);
        }
    }

    void Update()
    {
        float anguloOscilacion = limit * Mathf.Cos((Time.time * speed) + phaseOffset);
        
        transform.localRotation = Quaternion.Euler(startXRotation, startYRotation, startZRotation + anguloOscilacion);
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
        //rb.AddForce(direccionEmpuje * fuerzaDeEmpuje, ForceMode.Impulse);
    }
}
