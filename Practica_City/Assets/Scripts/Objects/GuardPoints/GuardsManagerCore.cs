using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GuardsManagerCore : MonoBehaviour
{
    //Singleton GuardManagerCore
    public static GuardsManagerCore Instance { get; private set; }
    public List<GuardPoint> listaGuardpoints;
    public List<GuardIA> guardias;
    public GameObject reserva;


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
   

    //#############################################
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

    public Vector3 solicitarPuntoDeEspera()
    {
        return reserva.transform.position;
    }

}
