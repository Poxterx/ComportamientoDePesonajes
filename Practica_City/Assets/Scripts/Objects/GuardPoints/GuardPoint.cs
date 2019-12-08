﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPoint : MonoBehaviour
{
    //Variables
    public int prioridad;
    public int peso = 0;
    public List<GuardPoint> vecinos = new List<GuardPoint>();
    public float rangoVecinos;
    private SphereCollider v_Collider;
    public bool ocupado = false;

    // Start is called before the first frame update
    void Start()
    {
        crearColliderRango();
        detectarVecinos();
        calcularPrioridad();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,rangoVecinos);
        Gizmos.color = Color.red;
        foreach(GuardPoint guard in vecinos)
        {
            Gizmos.DrawLine(this.gameObject.transform.position, guard.transform.position);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Guard")
        {
            ocupado = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Guard")
        {
            ocupado = false;
        }
    }

    private void crearColliderRango()
    {
        v_Collider = gameObject.AddComponent<SphereCollider>();
        v_Collider.center = Vector3.zero;
        v_Collider.radius = rangoVecinos;
        v_Collider.isTrigger = true;
    }

    private void detectarVecinos()
    {
        GameObject[] guardpoints = GameObject.FindGameObjectsWithTag("GuardPoint");
        foreach(GameObject guardPoint in guardpoints)
        {
            if (Vector3.Distance(guardPoint.transform.position,transform.position) <= rangoVecinos+guardPoint.GetComponent<GuardPoint>().rangoVecinos && guardPoint != this.gameObject)
            {
                //Añadir a una lista de tipo GuardPoint los gameobjects encontrados
                GuardPoint aux = guardPoint.GetComponent<GuardPoint>();
                vecinos.Add(aux);
            }
        }

    }

    private void calcularPrioridad()
    {
        prioridad = peso + vecinos.Count;
    }
}
