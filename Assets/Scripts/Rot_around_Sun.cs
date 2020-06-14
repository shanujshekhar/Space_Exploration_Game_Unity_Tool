using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rot_around_Sun : MonoBehaviour
{
    public GameObject sun;
    public float revolveSpeed;
    public float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OrbitAround();
        transform.RotateAround(transform.position, -Vector3.down, rotateSpeed * Time.deltaTime);
    }

    void OrbitAround()
    {
        transform.RotateAround(sun.transform.position, Vector3.up, revolveSpeed * Time.deltaTime);
    }
}
