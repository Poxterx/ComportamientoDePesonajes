using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestPoint : MonoBehaviour
{

    public float recoverRate = 3.0f;
        
    
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        // hambre -= desgaste * Time.deltaTime;
        GuardIA guardia = other.gameObject.GetComponent<GuardIA>();
        guardia.hambre += recoverRate * Time.deltaTime;
        guardia.energia += (recoverRate * 0.8f) * Time.deltaTime;
    }
}
