using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_collider_teleport : MonoBehaviour
{
    public Transform teleportLocation;
    public GameObject text;
    public GameObject Spaceship;
    // Start is called before the first frame update
    void Start()
    {
        text.SetActive(false);
    }

    void OnTriggerStay(Collider other)
    {
        //text.SetActive(true);
        Debug.Log("Portal_Collider Script");
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.position = teleportLocation.transform.position;
            transform.SetParent(Spaceship.transform);
            Debug.Log("Teleport Location: " + teleportLocation.transform.position);
            Debug.Log("Portal Location (After): " + transform.position);
        }
    }

    void OnTriggerExit()
    {
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
