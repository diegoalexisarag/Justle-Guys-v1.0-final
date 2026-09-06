using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Ragdoll : NetworkBehaviour
{
    [Header("Referencias")]
    public Animator animator;

    public Rigidbody hips;

    [Header("Configuración")]
    public float duracionRagdoll = 3f;

    public float multiplicadorImpulso = 3f;
    private Rigidbody[] rigidbodiesRagdoll;
    private Collider[] collidersRagdoll;

    private Rigidbody rbPrincipal;
    private Collider colPrincipal;
    private controlar movimiento;

    private bool enRagdoll = false;

    public bool EstaEnRagdoll => enRagdoll;


    private void Awake()
    {
        movimiento = GetComponent<controlar>();
        rbPrincipal = GetComponent<Rigidbody>();
        colPrincipal = GetComponent<Collider>();

        List<Rigidbody> rbs = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
        rbs.Remove(rbPrincipal);
        rigidbodiesRagdoll = rbs.ToArray();

        List<Collider> cols = new List<Collider>(GetComponentsInChildren<Collider>());
        cols.Remove(colPrincipal);
        collidersRagdoll = cols.ToArray();

        IgnorarColisionesInternas();

        PonerRagdollKinematico(true);
    }

    private void IgnorarColisionesInternas()
    {
        for (int i = 0; i < collidersRagdoll.Length; i++)
        {
            if (collidersRagdoll[i] == null) continue;

            if (colPrincipal != null)
            {
                Physics.IgnoreCollision(colPrincipal, collidersRagdoll[i], true);
            }

            for (int j = i + 1; j < collidersRagdoll.Length; j++)
            {
                if (collidersRagdoll[j] == null) continue;
                Physics.IgnoreCollision(collidersRagdoll[i], collidersRagdoll[j], true);
            }
        }
    }

    public void ActivarRagdoll(Vector3 impulso)
    {
        if (!IsOwner) return;
        if (enRagdoll) return;

        enRagdoll = true;

        if (animator != null) animator.enabled = false;
        if (movimiento != null) movimiento.canMove = false;
        if (rbPrincipal != null) rbPrincipal.isKinematic = true;
        if (colPrincipal != null) colPrincipal.enabled = false;

        PonerRagdollKinematico(false);

        if (hips != null)
        {
            //hips.AddForce(impulso, ForceMode.Impulse);
            hips.AddForce(impulso * multiplicadorImpulso, ForceMode.VelocityChange);
        }

        StartCoroutine(RutinaRecuperar());
    }

    private IEnumerator RutinaRecuperar()
    {
        yield return new WaitForSeconds(duracionRagdoll);
        DesactivarRagdoll();
    }

    private void DesactivarRagdoll()
    {
        PonerRagdollKinematico(true);

        if (hips != null)
        {
            transform.position = hips.position;
        }

        if (colPrincipal != null) colPrincipal.enabled = true;
        if (rbPrincipal != null) rbPrincipal.isKinematic = false;
        if (movimiento != null) movimiento.canMove = true;
        if (animator != null) animator.enabled = true;

        enRagdoll = false;
    }
    public void ReubicarInmediato(Vector3 posicion)
    {
        StopAllCoroutines();
 
        if (enRagdoll)
        {
            DesactivarRagdoll();
        }
 
        transform.position = posicion;
 
        if (hips != null)
        {
            hips.position = posicion;
            hips.linearVelocity = Vector3.zero;
            hips.angularVelocity = Vector3.zero;
        }
    }

    private void PonerRagdollKinematico(bool kinematico)
    {
        foreach (Rigidbody rb in rigidbodiesRagdoll)
        {
            if (rb != null) rb.isKinematic = kinematico;
        }

        foreach (Collider col in collidersRagdoll)
        {
            if (col != null) col.enabled = !kinematico;
        }
    }
}