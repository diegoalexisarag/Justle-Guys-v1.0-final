using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausaScript : MonoBehaviour
{
    public void VolverAlMenu()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
            Debug.Log("SESIÓN CERRADA");
        }
        SceneManager.LoadScene("MenuEscena");
    }
}
