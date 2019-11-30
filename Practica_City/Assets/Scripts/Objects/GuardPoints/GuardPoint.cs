using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPoint : MonoBehaviour
{
    //Variables
    public int prioridad;
    public List<GuardPoint> vecinos = new List<GuardPoint>();
    public float rangoVecinos;
    private SphereCollider v_Collider;
    public bool ocupado = false;

    // Start is called before the first frame update
    void Start()
    {
        crearColliderRango();
        detectarVecinos();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,rangoVecinos);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Guard"){
            
        }
    }

    private void crearColliderRango()
    {
        v_Collider = gameObject.AddComponent<SphereCollider>();
        v_Collider.center = Vector3.zero;
        v_Collider.radius = rangoVecinos;
    }

    private void detectarVecinos()
    {
        GameObject[] guardpoints = GameObject.FindGameObjectsWithTag("GuardPoint");
        foreach(GameObject guardPoint in guardpoints)
        {
            
        }

    }

    private void calcularNuevaPrioridad()
    {
        int nuevaPrioridad=0;
        if (!ocupado)
        {
            nuevaPrioridad = vecinos.Count;
        }

        foreach(GuardPoint vecino in vecinos)
        {
            if (!vecino.ocupado)
            {
                nuevaPrioridad += vecino.prioridad;
            }
            
        }

        prioridad = nuevaPrioridad;
    }
}
