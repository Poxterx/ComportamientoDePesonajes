using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//Esta clase gestiona los GuardPoints para que se les pueda asignar guardias
public class GuardsManagerCore : MonoBehaviour
{
    //Singleton GuardManagerCore
    public static GuardsManagerCore Instance { get; private set; }
    public List<GuardPoint> listaGuardpoints;
    public GameObject reserva;

    //Parte del Singleton
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        //Hacemos las listas de todos los elementos que vamos a usar
        montarListaGuardPoints();

        reserva = GameObject.FindGameObjectWithTag("Reserve");

    }

    //Encuentra todos los objetos con Tag GuardPoint y los añade a la lista para despues poder asignar a los guardias
    private void montarListaGuardPoints()
    {
        GameObject[] guardpoints = GameObject.FindGameObjectsWithTag("GuardPoint");
        foreach (GameObject guardPoint in guardpoints)
        {
            //Añadir a una lista de tipo GuardPoint los gameobjects encontrados
            GuardPoint aux = guardPoint.GetComponent<GuardPoint>();
            listaGuardpoints.Add(aux);
            
        }
        listaGuardpoints = listaGuardpoints.OrderByDescending(guardpoint => guardpoint.prioridad).ToList();
    }
   

    //Comprueba si encuentra un GuardPoint vacante, si no lo encuentra porque están todos cubiertos devuelve cero
    public Vector3 mirarTrabajoDisponible(GuardIA guardia)
    {
        int i = 0;
        while ( listaGuardpoints.Count > i)
        {
            if (listaGuardpoints[i].ocupado == false)
            {
                listaGuardpoints[i].tiempoEspera = 100.0f;
                listaGuardpoints[i].ocupado = true;
                listaGuardpoints[i].guardiaAsignado = guardia.gameObject.GetComponent<GuardIA>();
                //Encontrado trabajo y asignado
                return listaGuardpoints[i].transform.position;
            }
            i++; 
        }
        //No hay trabajo
        return Vector3.zero;
    }

    //Devuelve la posición del punto de espera para los guardias
    public Vector3 solicitarPuntoDeEspera()
    {
        return reserva.transform.position;
    }

}
