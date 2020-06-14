using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rot_with_spaceship : MonoBehaviour
{
    public GameObject spaceship;
    private Vector3 relativeDistance = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        relativeDistance = transform.position - spaceship.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = relativeDistance + spaceship.transform.position;
        relativeDistance = transform.position - spaceship.transform.position;
    }
}
