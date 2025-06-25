using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

public class CamaraLibreMouse : MonoBehaviour
{
    [SerializeField] ControladorDeCamaras controladorCamaras;

    [Header("Cámara libre a controlar")]
    [SerializeField] CinemachineCamera camaraLibre;

    [Header("Rig que rota con el mouse")]
    [SerializeField] Transform rigLibre;

    [Header("Configuración")]
    [SerializeField] float sensibilidad = 2f;
    [SerializeField] float limiteVertical = 80f;

    private float rotX = 0f, rotY = 0f;
    private bool vistaLibreActiva = false;
    private CinemachineCamera camaraAnterior;
    List<CinemachineCamera> todas;

    private void Start()
    {
        if(controladorCamaras != null) {
            todas = controladorCamaras.Cameras;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ActivarVistaLibre();

        if (Input.GetKeyUp(KeyCode.Space))
            DesactivarVistaLibre();

        if (vistaLibreActiva)
            MoverConMouse();
    }

    void ActivarVistaLibre()
    {
        vistaLibreActiva = true;

        foreach (var cam in todas)
        {
            if (cam != camaraLibre && cam.Priority == 10)
            {
                camaraAnterior = cam;
                cam.Priority = 0;
                break;
            }
        }

        camaraLibre.Priority = 10;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void DesactivarVistaLibre()
    {
        vistaLibreActiva = false;

        if (camaraAnterior != null)
            camaraAnterior.Priority = 10;

        camaraLibre.Priority = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void MoverConMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad;

        rotX -= mouseY;
        rotY += mouseX;
        rotX = Mathf.Clamp(rotX, -limiteVertical, limiteVertical);

        rigLibre.localRotation = Quaternion.Euler(rotX, rotY, 0f);
    }
}
