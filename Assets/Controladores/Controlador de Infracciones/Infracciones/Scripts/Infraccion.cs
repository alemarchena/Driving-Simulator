using System;
using UnityEngine;

[CreateAssetMenu(fileName ="Infraccion",menuName = "Scriptable Objects/Infracciones/Infraccion",order =0)]
public class Infraccion : ScriptableObject
{
    [SerializeField] InfraccionMonto infraccion;
    public AudioClip clip;

}

[Serializable]
struct InfraccionMonto
{
    public string nombre;
    [TextArea(3, 6)]  public string descripcion;
    public TypeInfraccion TypeInfraccion;
    public float monto;
    public Sprite imagenInfraccion;
    public Sprite gestoInfraccion;
}


public enum TypeInfraccion
{ leve, grave, muygrave, Imperdonable }
