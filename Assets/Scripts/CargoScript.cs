using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoScript : MonoBehaviour
{
    public GameObject cargo1;
    public GameObject cargo2;
    public GameObject cargo3;
    public GameObject cargo4;
    public GameObject spaceship;
    public GameObject text;
    public float floatSpeed;
    private bool move = false;
    private int count = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || OVRInput.GetDown(OVRInput.RawButton.A))
        {
            text.SetActive(false);
            move = true;
        }

        if (move == true)
            MoveCargo();
          
    }

    void MoveCargo()
    {
        switch(count)
        {
            case 1:
                if (cargo1.transform.parent.name == "Spaceship")
                {
                    move = false;
                    count++;
                }
                else
                {
                    cargo1.transform.Rotate(cargo1.transform.position, 5 * Time.deltaTime);
                    cargo1.transform.position = Vector3.MoveTowards(cargo1.transform.position, transform.Find("Cargo1TargetLocation").position, floatSpeed * Time.deltaTime);
                }
                break;
            case 2:
                
                if (cargo2.transform.parent.name == "Spaceship")
                {
                    move = false;
                    count++;
                }
                else
                {
                    //Debug.Log("Cargo2 : " + cargo2.transform.position);
                    //Debug.Log("Target : " + cargo2.transform.GetChild(1).transform.position);
                    cargo2.transform.Rotate(cargo2.transform.position, 5 * Time.deltaTime);
                    cargo2.transform.position = Vector3.MoveTowards(cargo2.transform.position, transform.Find("Cargo2TargetLocation").position, floatSpeed * Time.deltaTime);
                }
                break;
            case 3:
                if (cargo3.transform.parent.name == "Spaceship")
                {
                    move = false;
                    count++;
                }
                else
                {
                    //Debug.Log("Cargo2 : " + cargo3.transform.position);
                    //Debug.Log("Target : " + cargo3.transform.GetChild(1).transform.position);
                    cargo3.transform.Rotate(cargo3.transform.position, 5 * Time.deltaTime);
                    cargo3.transform.position = Vector3.MoveTowards(cargo3.transform.position, transform.Find("Cargo3TargetLocation").position, floatSpeed * Time.deltaTime);
                }
                break;
            case 4:
                if (cargo4.transform.parent.name == "Spaceship")
                {
                    move = false;
                    count++;
                }
                else
                {
                    //Debug.Log("Cargo2 : " + cargo4.transform.position);
                    //Debug.Log("Target : " + cargo4.transform.GetChild(1).transform.position);
                    cargo4.transform.Rotate(cargo4.transform.position, 5 * Time.deltaTime);
                    cargo4.transform.position = Vector3.MoveTowards(cargo4.transform.position, transform.Find("Cargo4TargetLocation").position, floatSpeed * Time.deltaTime);
                }
                break;
        }
    }

    //void MoveCargo()
    //{
    //    switch(count)
    //    {
    //        case 1: cargo1.transform.position = Vector3.MoveTowards(cargo1.transform.position, spaceship.transform.position, floatSpeed * Time.deltaTime);
    //                if (cargo1.transform.position == spaceship.transform.position)
    //                {
    //                    move = false;
    //                    text.SetActive(true);
    //                    count++;
    //                    cargo1.transform.SetParent(spaceship.transform);
    //                    //cargo1.transform.position = cargo1
    //                }
    //                break;
    //        case 2: cargo2.transform.position = Vector3.MoveTowards(cargo2.transform.position, spaceship.transform.position, floatSpeed * Time.deltaTime);
    //                if (cargo2.transform.position == spaceship.transform.position)
    //                {
    //                    move = false;
    //                    text.SetActive(true);
    //                    count++;
    //                    cargo2.transform.SetParent(spaceship.transform);
    //                }
    //                break;
    //        case 3: cargo3.transform.position = Vector3.MoveTowards(cargo3.transform.position, spaceship.transform.position, floatSpeed * Time.deltaTime);
    //                if (cargo3.transform.position == spaceship.transform.position)
    //                {
    //                    move = false;
    //                    text.SetActive(true);
    //                    count++;
    //                    cargo3.transform.SetParent(spaceship.transform);
    //                }
    //                break;
    //        case 4: cargo4.transform.position = Vector3.MoveTowards(cargo4.transform.position, spaceship.transform.position, floatSpeed * Time.deltaTime);
    //                if (cargo4.transform.position == spaceship.transform.position)
    //                {
    //                    move = false;
    //                    text.SetActive(true);
    //                    count++;
    //                    cargo4.transform.SetParent(spaceship.transform);
    //                }
    //                break;

    //    }
        
    //}

        
}
