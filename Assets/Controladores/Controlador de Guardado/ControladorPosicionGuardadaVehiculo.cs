using UnityEngine;

public class ControladorPosicionGuardadaVehiculo : MonoBehaviour
{

    private Transform vehiculo;
    [SerializeField] Transform posicionOriginal;

    [SerializeField] bool setearPosicionOriginal;
    private void Start()
    {
        vehiculo = FindAnyObjectByType<Vehiculo>().transform;

        if (setearPosicionOriginal)
            SetPosicionOriginal();
        else
            CargarPosicion();
    }
    public void CargarPosicion()
    {
        if (PlayerPrefs.HasKey("JugadorX"))
        {
            float x = PlayerPrefs.GetFloat("JugadorX");
            float y = PlayerPrefs.GetFloat("JugadorY");
            float z = PlayerPrefs.GetFloat("JugadorZ");
            float Rx = PlayerPrefs.GetFloat("JugadorRotationX");
            float Ry = PlayerPrefs.GetFloat("JugadorRotationY");
            float Rz = PlayerPrefs.GetFloat("JugadorRotationZ");

            if (vehiculo != null)
            {
                vehiculo.localPosition = new Vector3(x, y, z);
                vehiculo.localRotation = Quaternion.Euler(Rx, Ry, Rz);
            }
        }
    }


    public void SetPosicionOriginal()
    {
        vehiculo.localPosition = posicionOriginal.localPosition;
        vehiculo.localRotation = posicionOriginal.localRotation;
    }
}
