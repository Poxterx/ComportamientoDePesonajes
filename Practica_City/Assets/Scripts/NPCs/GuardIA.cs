using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Clase que contiene toda la lógica de los guardias.
public class GuardIA : MonoBehaviour
{
    NavMeshAgent _agent;

    //Necesidades del guardia
    public float hambre;
    public float energia;
    //Velocidad de consumo de las necesidades
    public const float desgaste = 0.5f;
    private CapsuleCollider colisionador;
    //Posición del objetivo actual
    private Vector3 currentTarget;
    //Comprobantes del estado del guardia
    public bool operativo = true;
    public bool asignado = false;

    // Start is called before the first frame update
    void Start()
    {
        _agent = this.GetComponent<NavMeshAgent>();
        hambre = Random.Range(70.0f, 100.0f);
        energia = Random.Range(70.0f, 100.0f);  
        colisionador = GetComponent<CapsuleCollider>();

        ignorarColisionGuardias();
    }

    // Update is called once per frame
    void Update()
    {
        maquinaFSM();
    }

    private void maquinaFSM()
    {
        //Las necesidades se van degradando con el tiempo
        consumo();
        //Comprobamos el estado en el que se encuentra actualmente
        comprobarEstado();

        //Despues de hacer mis chequeos...
        //Pregunto si hay trabajo disponible o me voy a cubrir mis necesidades
        //Si hay trabajo disponible, me voy a donde me manden
        //Si no hay trabajo disponible me voy a la reserva

        //Miro mi estado y decido si estoy dispuesto para trabajar
            if (operativo && !asignado)
            {
               Vector3 nuevoTrabajo = GuardsManagerCore.Instance.mirarTrabajoDisponible(this);
                if(nuevoTrabajo != Vector3.zero)
                {
                    //Vamos al punto de guardia asignado
                    asignado = true;
                    irAPosicionAsignada(nuevoTrabajo);
                }
                else
                {
                    //O nos vamos al punto de espera
                    irAPosicionAsignada( GuardsManagerCore.Instance.solicitarPuntoDeEspera() );
                }
            }
            //Cubro mis necesidades
            else if (!operativo)
            {
                asignado = false;
                irAPosicionAsignada( encontrarLugarDeDescansoMasCercano() ) ;
            }        
        
    }

    //Metodo usado para mover el agente a una nueva posición según la NavMesh
    public void irAPosicionAsignada(Vector3 nuevaPosicion)
    {
        currentTarget = nuevaPosicion;
        _agent.isStopped = false;
        if (Vector3.Distance(currentTarget, transform.position) > 3.0f)
        {
            _agent.SetDestination(nuevaPosicion);
        }
        else
        {
            _agent.isStopped = true;
        }
        
    }

    //Metodo para reducir las necesidades del guardia
    private void consumo()
    {            
        if (hambre < 0)
        {
            hambre = 0;
        }
        else
        {
            hambre -= desgaste * Time.deltaTime;
        }
        if (energia < 0)
        {
            energia = 0;
        }else
        {
            energia -= desgaste * Time.deltaTime;
        }
    }
    //Cambia los valores de estado del guardia
    private void comprobarEstado()
    {
        if (hambre>=70.0f && energia>=70.0f)
        {
            operativo = true;
        }
        else if (hambre<10 || energia<10)
        {
            operativo = false;
        }
    }
    //Metodo que hace que se ignoren mejor las colisiones con otros guardias
    private void ignorarColisionGuardias()
    {
        GameObject[] guards = GameObject.FindGameObjectsWithTag("Guard");
        foreach (GameObject guard in guards)
        {
            Physics.IgnoreCollision(guard.gameObject.GetComponent<CapsuleCollider>(), colisionador);
        }

    }
    //Metodo que cambia el objetivo a un area de descanso para suplir sus necesidades. Comprueba la más cercana en cada momento
    private Vector3 encontrarLugarDeDescansoMasCercano()
    {
        GameObject[] restAreas = GameObject.FindGameObjectsWithTag("RestArea");
        GameObject restAreaMasCercana = null;
        foreach (GameObject restArea in restAreas)
        {
            if (restAreaMasCercana == null)
            {
                restAreaMasCercana = restArea;
            }
            else if(Vector3.Distance(this.transform.position,restArea.transform.position) < Vector3.Distance(this.transform.position,restAreaMasCercana.transform.position))
            {
                restAreaMasCercana = restArea;
            }
        }
        return restAreaMasCercana.transform.position;
    }
}
