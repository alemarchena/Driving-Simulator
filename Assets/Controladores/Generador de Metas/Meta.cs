using UnityEngine;
using TMPro ;
using System.Collections.Generic;

public class Meta : MonoBehaviour
{
    [SerializeField] private string nombre;
    [SerializeField] private  TextMeshProUGUI nombreName;
    [SerializeField] private bool resolved=false;

    private GameObject vehiculo;
    private List<IMetaObserver> observers = new List<IMetaObserver>();

    public string Nombre => nombre;
    public bool Resolved => resolved;

    void Start()
    {
        vehiculo = FindAnyObjectByType<Vehiculo>().gameObject;
        nombreName.text = nombre;
    }

    void Update()
    {
        if (vehiculo != null)
        { 
            transform.LookAt(vehiculo.transform);
        }
    }

    public void Resolve()
    {
        if(resolved) return;

        resolved = true;

        MessageDisplaySystem.instance.ShowMessage("Meta:" + nombre +" resuelta",1f,0f);
        NotifyObserver();
    }

    public void AddObserver(IMetaObserver observer)
    {
        if(!observers.Contains(observer))
            observers.Add(observer);
    }

    public void RemoveObserver(IMetaObserver observer)
    {
        if(observers.Contains(observer))
            observers.Remove(observer);
    }

    public void NotifyObserver()
    {
        foreach(var observer in observers)
        {
            observer.OnMetaResolved(this);
        }
    }
}

