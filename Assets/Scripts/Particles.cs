using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{

    void Start()
    {

    }
    // Start is called before the first frame update
    void OnParticleCollision(GameObject other)
    {
        Debug.Log("Particle Hit" + other.transform.name);
        //if (other.gameObject.transform.name)
        //{
        //    Debug.Log("Collided Plant");
        //    other.gameObject.transform.localScale = other.gameObject.transform.localScale + other.gameObject.transform.forward;
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger : " + other.transform.name);
    }
}
