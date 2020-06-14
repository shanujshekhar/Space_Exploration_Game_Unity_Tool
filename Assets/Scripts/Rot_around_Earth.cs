using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rot_around_Earth : MonoBehaviour
{
    public GameObject Earth;
    private Vector3 relativeDistance = Vector3.zero;
    public float rotationSpeed;
    public Vector3 axis;

    // Start is called before the first frame update
    void Start()
    {
        if (Earth != null)
        {
            relativeDistance = transform.position - Earth.transform.position;
        }

    }
    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {   
        OrbitAround();
    }

    void OrbitAround()
    {
        if (Earth != null)
        {
            transform.position = Earth.transform.position + relativeDistance;
            transform.RotateAround(Earth.transform.position, axis, rotationSpeed * Time.deltaTime);
            relativeDistance = transform.position - Earth.transform.position;
        }
    }
}
