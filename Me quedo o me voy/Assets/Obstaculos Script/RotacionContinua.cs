using UnityEngine;

public class RotacionContinua : MonoBehaviour
{
    public float speed = 250f;
    
    public Vector3 ejeRotacion = Vector3.up;

    public float fuerzaDeEmpuje = 15f;
    public float fuerzaVertical = 4f;

    void Update()
    {
        transform.Rotate(ejeRotacion * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        controlar jugador = collision.gameObject.GetComponent<controlar>();
        if (jugador == null) return;

        if (!jugador.IsOwner) return;

        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb == null) return;

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