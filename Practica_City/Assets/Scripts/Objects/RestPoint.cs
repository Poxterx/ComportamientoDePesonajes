﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestPoint : MonoBehaviour
{
    //Velocidad de recuperación
    public float recoverRate = 3.0f;
        
    
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


     //El metodo incrementa la vida y energia de los guardias dentro de su area de efecto cada segundo.
    private void OnTriggerStay(Collider other)
    {
        GuardIA guardia = other.gameObject.GetComponent<GuardIA>();
        if (guardia.hambre<100.0f)
        {
            guardia.hambre += recoverRate * Time.deltaTime;
        }
        if (guardia.energia<100.0f)
        {
            guardia.energia += (recoverRate * 0.8f) * Time.deltaTime;
        }
        
    }
}
