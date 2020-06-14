using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WIM_movement : MonoBehaviour
{
    public GameObject player;
    
    void LateUpdate()
    {
        Vector3 newpos = transform.position;
        newpos.y = transform.position.y;
        transform.position = newpos;
    }
}
