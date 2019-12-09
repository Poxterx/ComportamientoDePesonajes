using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//esta clase contiene toda la lógica dentro de los puestos de guardia
public class GuardPoint : MonoBehaviour
{
    //Variables de prioridad
    public int prioridad;
    //Incremento de Prioridad manual.
    public int peso = 0;
    //Lista del resto de GuardPoints encontrados cerca
    public List<GuardPoint> vecinos = new List<GuardPoint>();
    //Rango del area de acción para detectar otros GuardPoints
    public float rangoVecinos;
    private SphereCollider v_Collider;
    //Referencia al guardia asignado
    public GuardIA guardiaAsignado = null;
    //Si se le ha asignado un guardia
    public bool ocupado = false;
    //Comprobante de la llegada del guardia
    public bool llegadaConfirmada = false;
    //Temporizador de cuanto se espera a la llegada del guardia
    public float tiempoEspera;

    // Start is called before the first frame update
    void Start()
    {
        crearColliderRango();
        detectarVecinos();
        calcularPrioridad();
    }

    // Update is called once per frame
    //Utilizado para ver si el guardia llega dentro del tiempo de espera y hacer funcionar el mismo
    void Update()
    {
        //Comprobamos si el tiempo de espera se ha agotado porque el guardia no ha llegado, para que se libere
        if (tiempoEspera<=0 && guardiaAsignado != null && !llegadaConfirmada)
        {
            ocupado = false;
            guardiaAsignado.asignado = false;
            guardiaAsignado = null;
        }

        tiempoEspera -= 1.0f*Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoVecinos);
        Gizmos.color = Color.red;
        foreach (GuardPoint guard in vecinos)
        {
            Gizmos.DrawLine(this.gameObject.transform.position, guard.transform.position);
        }

    }

    //Si llega el guardia asignado debemos gestionarlo
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<GuardIA>() == guardiaAsignado)
        {
            llegadaConfirmada = true;
        }
    }

    //Si sale el guardia asignado debemos gestionarlo
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<GuardIA>() == guardiaAsignado)
        {
            llegadaConfirmada = false;
            ocupado = false;
            guardiaAsignado = null;
            other.gameObject.GetComponent<GuardIA>().asignado = false;
        }
    }

    //Como dice el nombre, crea el collider dependiendo del rango que tenga el objeto
    private void crearColliderRango()
    {
        v_Collider = gameObject.AddComponent<SphereCollider>();
        v_Collider.center = Vector3.zero;
        v_Collider.radius = rangoVecinos;
        v_Collider.isTrigger = true;
    }

    //A la hora de cargar la escena, cada punto comprueba si tiene más puntos de guardia dentro de su rango para tomarlos como vecinos.
    private void detectarVecinos()
    {
        GameObject[] guardpoints = GameObject.FindGameObjectsWithTag("GuardPoint");
        foreach (GameObject guardPoint in guardpoints)
        {
            if (Vector3.Distance(guardPoint.transform.position, transform.position) <= rangoVecinos + guardPoint.GetComponent<GuardPoint>().rangoVecinos && guardPoint != this.gameObject)
            {
                //Añadir a una lista de tipo GuardPoint los gameobjects encontrados
                GuardPoint aux = guardPoint.GetComponent<GuardPoint>();
                vecinos.Add(aux);
            }
        }

    }

    //Dependiendo de los vecinos y el peso manual, la prioridad será mayor o menor
    private void calcularPrioridad()
    {
        prioridad = peso + vecinos.Count;
    }
}
