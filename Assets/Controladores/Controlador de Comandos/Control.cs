using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control es una clase que permite asignar un comando (ej:tecla) a una clave conformada por ParteSubparte
/// </summary>
[Serializable]
public class Control
{
    /// <summary>
    /// Parte y sub parte del Vehículo
    /// </summary>
    public ParteSubParte parteSubparte;

    /// <summary>
    /// Comando para controlar la parte
    /// </summary>
    public List<KeyCode> teclas;
}


/// <summary>
/// ParteSubParte es una clase que consta de "PartesComando" del vehículo, y "Subpartes"
/// </summary>
[Serializable]
public class ParteSubParte
{
    /// <summary>
    /// Parte del vehículo que se desea controlar mediante un comando
    /// </summary>
    public enum PartesComando
    {
        Acelerador,
        Freno,
        Balizas,
        Bocina,
        LimpiaParabrisas,
        Radio,
        Embrague,
        PalancaCambios,
        CinturonSeguridad,
        LuzAltaBaja,
        Nitro,
        Intermitente,
        Camara,
        RuedaAuxilio,
        Tanque,
        Volante,
        FrenoMano,
        Motor
    }

    /// <summary>
    /// La "Subparte" brinda la posibilidad de manejar un comando diferente para una "Parte" asignada
    /// </summary>
    public enum SubParte
    {
        Ninguna,
        Derecha, Izquierda,
        Neutro,Reversa, Primera, Segunda, Tercera, Cuarta, Quinta, Sexta,
        Subir, Bajar,
    }

    public PartesComando parte;
    public SubParte subParte;
}
