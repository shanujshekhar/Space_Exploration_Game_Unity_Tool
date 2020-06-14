using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public GameObject Collider1;
    public GameObject Collider2;
    public GameObject Earth;
    public float revolutionSpeed;
    public Vector3 axis;
    private Vector3 relativeDistance = Vector3.zero;
    private float rotateSpeed = 20;
    private float upSpeed = 5;
    private float downSpeed = 50;
    private Rigidbody rigBody;
    private bool collided = false;

    // Start is called before the first frame update
    void Start()
    {
        relativeDistance = transform.position - Earth.transform.position;
        rigBody = transform.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) || OVRInput.GetUp(OVRInput.RawButton.Y))
        {
            rigBody.useGravity = !rigBody.useGravity;
        }
        if(rigBody.useGravity == false )           
            RotateObject();
    }

    void RotateObject()
    {
        transform.RotateAround(transform.position, new Vector3(1, 1, 1), rotateSpeed * Time.deltaTime);
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

            if( rigBody.useGravity==true )
                transform.position = Vector3.MoveTowards(transform.position, Collider1.transform.position, downSpeed * Time.deltaTime);

            if( collided==true && rigBody.useGravity==false )
                transform.position = Vector3.MoveTowards(transform.position, Collider2.transform.position, upSpeed * Time.deltaTime);

            transform.RotateAround(Earth.transform.position, axis, revolutionSpeed * Time.deltaTime);
            relativeDistance = transform.position - Earth.transform.position;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        collided = true;
        Debug.Log(collided);
    }
}
