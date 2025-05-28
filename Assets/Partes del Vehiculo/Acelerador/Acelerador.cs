using UnityEngine;

public class Acelerador : Simulator
{
    [SerializeField] private float aceleracion;
    [SerializeField] private float tiempoHastaMaximaAceleracion = 3f;

    private float MaxAceleracion = 1.0f;
    private float MinAceleracion = 0.0f;
    [SerializeField] private float tasaIncremento = 0.05f; // Por segundo
    [SerializeField] private float tasaDecremento = 1.0f; // Por segundo
    [SerializeField] private Creadores creadores = Creadores.Diaz_Corvalan_Matias_Federico;

    private void Awake()
    {
        creadores = Creadores.Diaz_Corvalan_Matias_Federico;
    }
    private void Start()
    {
        AsignarCreador(creadores);
        AsignarComandos();
        Describir();
    }
   

    public override void AsignarCreador(Creadores creador)
    {
        CreadoresSimulator = creador;
    }

    void Update()
    {
        Actualizar(Time.deltaTime);
    }

    public void Actualizar(float deltaTime)
    {
        float valorAnterior = aceleracion;
        tasaIncremento = 1f / tiempoHastaMaximaAceleracion;

        if (SeMantienePresionadaLaTecla())
        {
            aceleracion += tasaIncremento * deltaTime;
            if (aceleracion > MaxAceleracion)
                aceleracion = MaxAceleracion;
        }
        else
        {
            aceleracion -= tasaDecremento * deltaTime;
            if (aceleracion < MinAceleracion)
                aceleracion = MinAceleracion;
        }
        //if (valorAnterior != aceleracion)
        //{
        //    Debug.Log($"[DEBUG] Aceleracion: {aceleracion:F2}");
        //}
    }
    public override void Describir()
    {
    }
    public float GetAceleracion()
    {
        return aceleracion;
    }
}
