using UnityEngine;
using TMPro;

public class ItemInfraccion : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _nombre;
    [SerializeField] TextMeshProUGUI _monto;
    [SerializeField] TextMeshProUGUI _tipo;    


    public void SetTextInfraccion(string nombre, string monto, string tipo)
    {
        _nombre.text = nombre;
        _monto.text = monto;
        _tipo.text = tipo;
    }

    public string Nombre => _nombre.text;
    public string Monto => _monto.text;
    public string Tipo => _tipo.text;
}
