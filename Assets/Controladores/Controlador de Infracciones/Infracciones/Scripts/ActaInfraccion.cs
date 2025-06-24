using UnityEngine;

public class ActaInfraccion : MonoBehaviour
{
    [SerializeField] Infraccion infraccion;

    public void CrearActaInfraccion()
    {
        if(infraccion != null)
        {
            Supervisor.Instance.AgregarInfraccion(infraccion);
            NotificarSonidoInfraccion();
        }
    }

    public void NotificarSonidoInfraccion()
    {
        if(infraccion.clip != null)
        {
            ControladorSonidos.Instance.PlaySoundGlobal(infraccion.clip, ControladorSonidos.ModePlay.playOneShoot, false, 1f);
        }
    }
}
