using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardIA : MonoBehaviour
{
    NavMeshAgent _agent;

    public float hambre;
    public float energia;
    public const float desgaste = 0.5f;
    private CapsuleCollider colisionador;
    public GameObject restArea;

    public bool operativo = true;
    public bool asignado = false;

    // Start is called before the first frame update
    void Start()
    {
        _agent = this.GetComponent<NavMeshAgent>();
        hambre = Random.Range(70.0f, 100.0f);
        energia = Random.Range(70.0f, 100.0f);  
        colisionador = GetComponent<CapsuleCollider>();
        restArea = GameObject.FindGameObjectWithTag("RestArea");
    }

    // Update is called once per frame
    void Update()
    {
        //Las necesidades se van degradando con el tiempo
        consumo();
        //Comprobamos el estado en el que se encuentra actualmente
        comprobarEstado();
    }

    public void irAPosicionAsignada(Vector3 nuevaPosicion)
    {
        _agent.SetDestination(nuevaPosicion);
    }

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

    private void ignorarColisionGuardias()
    {
        GameObject[] guards = GameObject.FindGameObjectsWithTag("Guard");
        foreach (GameObject guard in guards)
        {
            Physics.IgnoreCollision(guard.gameObject.GetComponent<CapsuleCollider>(), colisionador);
        }

    }
}
