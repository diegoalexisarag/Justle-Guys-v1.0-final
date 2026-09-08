using UnityEngine;
using UnityEngine.SceneManagement;
public class RegresarAlMenu : MonoBehaviour
{
    [Header("Configuración de Paneles")]
    [Tooltip("El panel de UI que se va a ocultar")]
    [SerializeField] private GameObject panelActual;
    
    [Tooltip("El panel que se va a mostrar (Menú Principal)")]
    [SerializeField] private GameObject panelDestino;

    public void CambiarPanel()
    {
        if (panelActual != null) panelActual.SetActive(false);
        if (panelDestino != null) panelDestino.SetActive(true);
    }
}
