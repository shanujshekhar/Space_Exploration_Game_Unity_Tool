using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public GameObject Sun;
    public GameObject ReferenceObject;
    public GameObject Light;
    private Vector3 direction = Vector3.zero;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        Light.GetComponent<Light>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        direction = ReferenceObject.transform.position - Sun.transform.position;

        if ( Physics.Raycast(Sun.transform.position, direction, out hit) )
        {
            Debug.Log(hit.collider.gameObject.name);
            if ( !(hit.collider.gameObject.name == "Can") )
            {
                Light.GetComponent<Light>().enabled = true;
                Debug.DrawRay(Sun.transform.position, direction, Color.green);
                Debug.Log("HIT");
            }
            else
                Light.GetComponent<Light>().enabled = false;
        }
        else
            Light.GetComponent<Light>().enabled = true;
    }
}
