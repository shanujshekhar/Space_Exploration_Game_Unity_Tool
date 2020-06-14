using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMovement : MonoBehaviour
{
    public float LRspeed;
    public float FBspeed;
    public float handleSpeed = 2;
    public OVRCameraRig CameraRig;
    public GameObject cargoMenu;
    public GameObject teleportText;
    private Teleport teleportScript;
    private bool left = false;
    private bool right = false;
    public bool forward = false;
    private bool backward = false;
    private bool raycast = false;
    private bool movecargo = false;
    private bool movecargodown = false;
    private bool movecargoup = false;
    private bool textActive = false;
    private float distance = 100000;
    private Vector2 leftJoystick;
    private Vector2 rightJoystick;
    private GameObject hitObject;
    private Transform handle;
    private Transform leftController;
    private Transform rightController;
    private LineRenderer LeftRender;
    private LineRenderer RightRender;
    private RaycastHit hitleft;
    private RaycastHit hitright;
    private TextMesh tm;
    private float delay = 0.1f;
    private float curTime = 0;
    private int count = 1;
    private Quaternion origRot;

    // Start is called before the first frame update
    void Start()
    {
       

        leftController = CameraRig.leftHandAnchor.GetChild(1);
        rightController = CameraRig.rightHandAnchor.GetChild(1);
        
        LeftRender = leftController.GetComponent<LineRenderer>();
        RightRender = rightController.GetComponent<LineRenderer>();
        handle = transform.GetChild(0).GetChild(3).GetChild(2).GetChild(1);
        cargoMenu.SetActive(true);
        tm = cargoMenu.GetComponent<TextMesh>();
        tm.text = "Are you ready to Fly the Spaceship?" + System.Environment.NewLine + "Press X to Continue.";
        origRot = handle.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (!textActive)
            {
                teleportScript = transform.GetComponent<Teleport>();
                teleportScript.enabled = false;
                cargoMenu.SetActive(true);
            }

            //OVRInput.RawAxis2D.PrimaryThumbstick;
            leftJoystick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            rightJoystick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

            if (Input.GetKeyDown(KeyCode.X) || OVRInput.GetDown(OVRInput.RawButton.X))
                tm.text = "Press LCtrl or A to accept Cargo." + System.Environment.NewLine + "Press G or B to discard request.";

            if (Input.GetKeyDown(KeyCode.LeftControl) || OVRInput.GetDown(OVRInput.RawButton.A))
            {
                raycast = true;
                cargoMenu.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.G) || OVRInput.GetDown(OVRInput.RawButton.B))
            {
                tm.text = "Use W, A, S, D or Left Joystick to Fly Spaceship";
                //cargoMenu.SetActive(false);
                teleportScript.enabled = true;
                textActive = true;
                LeftRender.positionCount = 0;
                RightRender.positionCount = 0;
                raycast = false;
            }

            if (raycast)
                CastRay();

            if (movecargo)
                MoveCargo();

            if (Input.GetKey(KeyCode.A) || (leftJoystick.x < -0.7f && (leftJoystick.y > 0.0f && leftJoystick.y < 0.5f)))
                left = true;

            if (Input.GetKey(KeyCode.D) || (leftJoystick.x > 0.7f && (leftJoystick.y < 0.0f && leftJoystick.y > -0.5f)))
                right = true;

            if (Input.GetKey(KeyCode.W) || (leftJoystick.y > 0.7f && (leftJoystick.x > 0.0f && leftJoystick.x < 0.5f)))
                forward = true;

            if (Input.GetKey(KeyCode.S) || (leftJoystick.y < -0.7f && (leftJoystick.x < 0.0f && leftJoystick.x > -0.3f)))
                backward = true;

            if (left)
                MoveLeft();

            if (right)
                MoveRight();

            if (forward)
                MoveForward();

            if (backward)
                MoveBackward();
        }catch(Exception e)
        {
            tm.text = "SpaceShipMenu Message: " + e.StackTrace;
        }
    }


    void CastRay()
    {
        //LeftRender.SetPosition(0, leftController.transform.position);
        //LeftRender.SetPosition(1, leftController.transform.forward * distance);
        //RightRender.SetPosition(0, rightController.transform.position);
        //RightRender.SetPosition(1, rightController.transform.forward * distance);

        if (Physics.Raycast(leftController.transform.position, leftController.transform.forward, out hitleft) && Physics.Raycast(rightController.transform.position, rightController.transform.forward, out hitright))
        {
            Debug.Log(hitleft.collider.name);
            if (hitleft.collider.name.StartsWith("cargo") && hitright.collider.name.StartsWith("cargo"))
            {
                hitleft.collider.transform.parent.transform.SetParent(transform);
                hitObject = hitleft.collider.transform.parent.gameObject;

                hitObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                //StartCoroutine(Delay());
                //hitObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");

                //Debug.Log("Parent of " + hitObject.name + " : " + hitObject.transform.parent.name);
                movecargo = true;

                LeftRender.SetPosition(0, leftController.transform.position);
                LeftRender.SetPosition(1, hitleft.collider.transform.position);
                RightRender.SetPosition(0, rightController.transform.position);
                RightRender.SetPosition(1, hitleft.collider.transform.position);

                Debug.DrawLine(leftController.transform.position, hitleft.collider.transform.position);
                Debug.DrawLine(rightController.transform.position, hitright.collider.transform.position);
            }
            else
            {
                movecargo = false;
                LeftRender.SetPosition(0, leftController.transform.position);
                LeftRender.SetPosition(1, leftController.transform.forward * distance);
                RightRender.SetPosition(0, rightController.transform.position);
                RightRender.SetPosition(1, rightController.transform.forward * distance);

                Debug.DrawLine(leftController.transform.position, leftController.transform.forward * distance, Color.green);
                Debug.DrawLine(rightController.transform.position, rightController.transform.forward * distance, Color.green);
            }

        }
        else
        {
            LeftRender.SetPosition(0, leftController.transform.position);
            LeftRender.SetPosition(1, leftController.transform.forward * distance);
            RightRender.SetPosition(0, rightController.transform.position);
            RightRender.SetPosition(1, rightController.transform.forward * distance);

            Debug.DrawLine(leftController.transform.position, leftController.transform.forward * distance, Color.green);
            Debug.DrawLine(rightController.transform.position, rightController.transform.forward * distance, Color.green);
        }
    }

    void MoveCargo()
    {
        tm.text = "Use Right JoyStick or Q to Move Object";

        if (rightJoystick.y < 0.0f || Input.GetKey(KeyCode.Q))
        {
            hitObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            hitObject.transform.position = Vector3.MoveTowards(hitObject.transform.position, RightRender.GetPosition(0), 90 * Time.deltaTime);
        }
        else if (rightJoystick.y > 0.0f)
        {
            hitObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            hitObject.transform.position = Vector3.MoveTowards(hitObject.transform.position, RightRender.GetPosition(1), 90 * Time.deltaTime);
        }
        else
            hitObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");

        //hitObject.transform.position = Vector3.MoveTowards(hitObject.transform.position, RightRender.GetPosition(0), 25 * Time.deltaTime);

        if (Vector3.Distance(hitObject.transform.position, RightRender.GetPosition(0)) < 50)
        {
            hitObject.SetActive(false);
            cargoMenu.SetActive(true);
            if (count <= 4)
            {
                if (count == 4)
                    tm.text = "All Cargos Loaded";
                else
                    tm.text = "Cargo (" + count + "/4) Loaded Successfully!!!!";

                count++;
            }

            StartCoroutine(MyMethod());
            movecargo = false;
        }
    }

    //IEnumerator Delay()
    //{
    //    Debug.Log("Before Waiting 0.5 seconds");
    //    yield return new WaitForSeconds(1);
    //    Debug.Log("After Waiting 0.5 seconds");
    //}

    IEnumerator MyMethod()
    {
        Debug.Log("Before Waiting 2 seconds");
        yield return new WaitForSeconds(2);
        tm.text = "Press LCtrl or A to accept Cargo." + System.Environment.NewLine + "Press G or B to discard request.";
        Debug.Log("After Waiting 2 Seconds");
    }

    void MoveLeft()
    {
        transform.RotateAround(-Vector3.up, LRspeed * Time.deltaTime);

        if (curTime == 0)
            curTime += Time.time;

        if (Time.time < curTime + delay)
            handle.RotateAround(transform.forward, handleSpeed * Time.deltaTime);

        if (leftJoystick.x == 0.0f && leftJoystick.y == 0.0f)
        {
            handle.rotation = transform.rotation;
            curTime = 0;
            left = false;
        }
    }

    void MoveRight()
    {
        transform.RotateAround(Vector3.up, LRspeed * Time.deltaTime);

        if (curTime == 0)
            curTime += Time.time;

        if (Time.time < curTime + delay)
            handle.RotateAround(-transform.forward, handleSpeed * Time.deltaTime);

        if (leftJoystick.x == 0.0f && leftJoystick.y == 0.0f)
        {
            handle.rotation = transform.rotation;
            right = false;
            curTime = 0;
        }
    }


    void MoveForward()
    {
        transform.position += transform.forward * Time.deltaTime * FBspeed;

        if (curTime == 0)
            curTime += Time.time;

        if (Time.time < curTime + delay)
            handle.RotateAround(transform.right, handleSpeed * Time.deltaTime);

        if (leftJoystick.x == 0.0f && leftJoystick.y == 0.0f)
        {
            handle.rotation = transform.rotation;
            forward = false;
            curTime = 0;
        }
    }

    void MoveBackward()
    {
        transform.position -= transform.forward * Time.deltaTime * FBspeed;

        if (curTime == 0)
            curTime += Time.time;

        if (Time.time < curTime + delay)
            handle.RotateAround(-transform.right, handleSpeed * Time.deltaTime);

        if (leftJoystick.x == 0.0f && leftJoystick.y == 0.0f)
        {
            handle.rotation = transform.rotation;
            curTime = 0;
            backward = false;
        }
    }




    //******************************************************************************************************//
    // Keyboard

    //void MoveLeft()
    //{
    //    transform.RotateAround(-Vector3.up, LRspeed * Time.deltaTime);
    //    if (curTime == 0)
    //        curTime += Time.time;

    //    if (Time.time < curTime + delay)
    //        handle.RotateAround(transform.forward, handleSpeed * Time.deltaTime);

    //    if (Input.GetKeyUp(KeyCode.A))
    //    {
    //        handle.rotation = transform.rotation;
    //        curTime = 0;
    //        left = false;
    //    }
    //}

    //void MoveRight()
    //{
    //    transform.RotateAround(Vector3.up, LRspeed * Time.deltaTime);

    //    if (curTime == 0)
    //        curTime += Time.time;

    //    if (Time.time < curTime + delay)
    //        handle.RotateAround(-transform.forward, handleSpeed * Time.deltaTime);

    //    if (Input.GetKeyUp(KeyCode.D))
    //    {
    //        handle.rotation = transform.rotation;
    //        right = false;
    //        curTime = 0;
    //    }
    //}


    //void MoveForward()
    //{
    //    transform.position += transform.forward * Time.deltaTime * FBspeed;

    //    if (curTime == 0)
    //        curTime += Time.time;

    //    if (Time.time < curTime + delay)
    //        handle.RotateAround(transform.right, handleSpeed * Time.deltaTime);

    //    if (Input.GetKeyUp(KeyCode.W))
    //    {
    //        handle.rotation = transform.rotation;
    //        forward = false;
    //        curTime = 0;
    //    }
    //}

    //void MoveBackward()
    //{
    //    transform.position -= transform.forward * Time.deltaTime * FBspeed;

    //    if (curTime == 0)
    //        curTime += Time.time;

    //    if (Time.time < curTime + delay)
    //        handle.RotateAround(-transform.right, handleSpeed * Time.deltaTime);

    //    if (Input.GetKeyUp(KeyCode.S))
    //    {
    //        handle.rotation = transform.rotation;
    //        curTime = 0;
    //        backward = false;
    //    }
    //}
}
