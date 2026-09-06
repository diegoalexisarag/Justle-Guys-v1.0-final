using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Configuración de Reaparición")]
    [SerializeField] private float limiteCaida = -0.1f;
    [SerializeField] public Transform puntoDeReaparicion;

    private CharacterController characterController;
    private Ragdoll ragdoll;
    private Vector3 posicionInicial;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        ragdoll = GetComponent<Ragdoll>();
        posicionInicial = transform.position;
    }

    void Update()
    {
        float posicionY = ObtenerPosicionYActual();
        if (posicionY < limiteCaida)
        {
            Reaparecer();
        }
    }
        private float ObtenerPosicionYActual()
    {
        if (ragdoll != null && ragdoll.EstaEnRagdoll && ragdoll.hips != null)
        {
            return ragdoll.hips.position.y;
        }

        return transform.position.y;
    }

    public void Reaparecer()
    {
        Vector3 posicionDestino = puntoDeReaparicion != null ? puntoDeReaparicion.position : posicionInicial;
        
        if (ragdoll != null)
        {
            ragdoll.ReubicarInmediato(posicionDestino);
        }

        if (characterController != null)
        {
            characterController.enabled = false;
            transform.position = posicionDestino;
            characterController.enabled = true;
        }
        else
        {
            transform.position = posicionDestino;
        }
    }

   private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detectado con: " + other.gameObject.name + " | Tag: " + other.tag);
        if (other.CompareTag("obstaculo-letal"))
        {
            Reaparecer();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Choque detectado con: " + hit.gameObject.name + " | Tag: " + hit.gameObject.tag);
        if (hit.gameObject.CompareTag("obstaculo-letal"))
        {
            Reaparecer();
        }
    }
}