using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    public Transform teleportLocation;
    public Canvas canvas;
    public GameObject teleportText;
    public GameObject instructions;
    public GameObject cargoMenu;
    public GameObject TaskMenu;
    public GameObject Planet;
    public GameObject cart;
    public GameObject cargo1;
    public GameObject cargo2;
    public GameObject cargo3;
    public GameObject cargo4;
    public OVRCameraRig CameraRig;
    public Transform cameraRotate;
    public Transform cargo1Loc;
    public Transform cargo2Loc;
    public Transform cargo3Loc;
    public Transform cargo4Loc;
    public Transform portalSpaceshipLocation;
    public Transform cameraSpaceshipLocation;
    private bool raycast = false;
    private SpaceshipMovement SpaceshipMovementScript;
    private bool moveCamera = false;
    private bool hit = false;
    private bool cameraMoved = false;
    private bool changeScript = false;
    private bool task = false;
    private bool task1 = false;
    private bool task2 = false;
    private bool objectselected = false;
    private bool teleportBack = false;
    private bool togglelight = false;
    private float distance = 100000;
    private TextMesh tm;
    private TextMesh instr;
    private Transform portal;
    private Vector2 leftJoystick;
    private Vector2 rightJoystick;
    private Vector3 portalOriginal;
    private Vector3 cameraOriginal;
    private Vector3 origPos;
    private Quaternion origRot;
    private Transform leftController;
    private Transform rightController;
    private LineRenderer LeftRender;
    private LineRenderer RightRender;
    private RaycastHit hitleft;
    private RaycastHit hitright;
    private GameObject selectedObject = null;
    private List<string> lights = new List<string>();
    private float reldist = 0;
    private bool ExceptionOccured = false;
    private string ExceptionMessage = "";
    private int layerMask;
    private int lightOn = 0;
    private int cargoLoaded = 0;
    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << 8;
        layerMask = ~layerMask;

        portal = transform.Find("portal");

        leftController = CameraRig.leftHandAnchor.GetChild(1);
        rightController = CameraRig.rightHandAnchor.GetChild(1);

        cart = Planet.transform.FindChildRecursive("cart").gameObject;

        LeftRender = leftController.GetComponent<LineRenderer>();
        RightRender = rightController.GetComponent<LineRenderer>();

        tm = teleportText.GetComponent<TextMesh>();
        instr = teleportText.GetComponent<TextMesh>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!changeScript)
        {
            cargoMenu.SetActive(false);
            SpaceshipMovementScript = transform.GetComponent<SpaceshipMovement>();
            SpaceshipMovementScript.forward = false;
            SpaceshipMovementScript.enabled = false;
        }

        Debug.Log("OnTriggerEnter: " + other.name);
        moveCamera = true;
        
    }

    void Update()
    {
        try
        {
            canvas.gameObject.SetActive(false);
            leftJoystick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            rightJoystick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);


            if (moveCamera == true)
            {

                tm.text = "                 Use Right Joystick to go to Portal Device";

                if( rightJoystick.x < 0.0f )
                    CameraRig.transform.position = Vector3.MoveTowards(CameraRig.transform.position, portal.transform.position, 10 * Time.deltaTime);
                else if(rightJoystick.x > 0.0f )
                    CameraRig.transform.position = Vector3.MoveTowards(CameraRig.transform.position, rightController.transform.position, 10 * Time.deltaTime);
                //else
                //    CameraRig.transform.position = Vector3.MoveTowards(CameraRig.transform.position, portal.transform.position, 10 * Time.deltaTime);

                if (Vector3.Distance(portal.transform.position, CameraRig.transform.position) < 20)
                {
                    moveCamera = false;
                    cameraMoved = true;
                }
            }

            if (cameraMoved)
            {
                tm.text = "                 Press E or B to Teleport on PlanetCSE566";
                if (Input.GetKeyDown(KeyCode.E) || (OVRInput.GetDown(OVRInput.RawButton.B)))
                {
                    origPos = CameraRig.transform.position;
                    origRot = CameraRig.transform.rotation;
                    CameraRig.transform.position = teleportLocation.transform.position;
                    CameraRig.transform.rotation = teleportLocation.transform.rotation;
                    cameraMoved = false;
                    raycast = true;
                }
            }

            if (raycast)
                Raycast();

            if (hit && (Input.GetKeyDown(KeyCode.X) || (OVRInput.GetDown(OVRInput.RawButton.X))))
            {
                
                LeftRender.positionCount = 0;
                portal.transform.position = new Vector3(hitleft.point.x, hitleft.point.y + 3, hitleft.point.z);
                portal.transform.rotation = cameraRotate.transform.rotation;
                CameraRig.transform.position = new Vector3(hitleft.point.x, hitleft.point.y + 50, hitleft.point.z);
                CameraRig.transform.LookAt(cameraRotate);

                task = true;
                hit = false;
                raycast = false;
            }

            if (task && !teleportBack)
            {
                LeftRender.positionCount = 2;
                RightRender.positionCount = 2;
                Tasks();
            }

            if (selectedObject && reldist != 0)
            {
                reldist += leftJoystick.y;
                selectedObject.transform.position = leftController.transform.position + (leftController.transform.forward * reldist);
            }

            if (teleportBack)
            {
                LeftRender.positionCount = 0;
                RightRender.positionCount = 0;
                TaskMenu.SetActive(false);
                tm.text = "  Go to the Portal Device" + System.Environment.NewLine + "Use Right Joystick to Move the Player";

                if (rightJoystick.y < 0.0f)
                    CameraRig.transform.position = Vector3.MoveTowards(CameraRig.transform.position, -CameraRig.transform.forward, 10 * Time.deltaTime);
                else if (rightJoystick.y > 0.0f)
                    CameraRig.transform.position = Vector3.MoveTowards(CameraRig.transform.position, CameraRig.transform.forward, 10 * Time.deltaTime);

                if (Vector3.Distance(CameraRig.transform.position, portal.transform.position) < 37)
                {
                    tm.text = "                 Press Y to Teleport";
                    if (Input.GetKeyDown(KeyCode.Y) || (OVRInput.GetDown(OVRInput.RawButton.Y)))
                    {
                        Debug.Log("Teleport Back");
                        

                        CameraRig.transform.position = cameraSpaceshipLocation.position;
                        CameraRig.transform.rotation = cameraSpaceshipLocation.rotation;
                        portal.transform.position = portalSpaceshipLocation.position;
                        portal.transform.rotation = portalSpaceshipLocation.rotation;

                        leftController.rotation = cameraSpaceshipLocation.rotation;
                        rightController.rotation = cameraSpaceshipLocation.rotation;

                        changeScript = true;

                        SpaceshipMovementScript = transform.GetComponent<SpaceshipMovement>();
                        SpaceshipMovementScript.enabled = true;

                        SpaceshipMovementScript.cargoMenu.GetComponent<TextMesh>().gameObject.SetActive(true);
                        SpaceshipMovementScript.cargoMenu.GetComponent<TextMesh>().text = "Use W, A, S, D or Left Joystick to Fly Spaceship";

                        teleportText.SetActive(false);

                        canvas.gameObject.SetActive(true);

                        transform.GetComponent<Teleport>().enabled = false;
                    }
                }
            }
            if (ExceptionOccured)
            {
                tm.text = "Updata(): " + ExceptionMessage;
            }
        }catch(Exception e)
        {
            ExceptionMessage = e.Message;
            ExceptionOccured = true;
        }
    }

    void Raycast()
    {
        tm.text = "Move Left Controller Anywhere in Green Area To Teleport" + System.Environment.NewLine + "Press X to Confirm.";
        
        LeftRender.positionCount = 2;
        LeftRender.SetPosition(0, leftController.transform.position);
        LeftRender.SetPosition(1, leftController.transform.forward * distance);

        if (Physics.Raycast(leftController.transform.position, leftController.transform.forward, out hitleft, Mathf.Infinity, layerMask))
        {
            if (hitleft.collider.gameObject.name == "GreenHouseBase")
            {
                hit = true;
            }
        }
    }
    
    void Tasks()
    {
        tm.text = "";

        LeftRender.SetPosition(0, leftController.transform.position);
        LeftRender.SetPosition(1, leftController.transform.forward * distance);

        RightRender.SetPosition(0, rightController.transform.position);
        RightRender.SetPosition(1, rightController.transform.forward * distance);

        if (task1 || task2)
        {
            if (task1)
                Task1();
            else if (task2)
                Task2();
        }
        else
        {

            tm.text = "Use Both Controllers to Select Task";

            TaskMenu.SetActive(true);
            RaycastHit hitleft;
            RaycastHit hitright;

            if (Physics.Raycast(leftController.transform.position, leftController.transform.forward, out hitleft, Mathf.Infinity, layerMask) && Physics.Raycast(rightController.transform.position, rightController.transform.forward, out hitright, Mathf.Infinity, layerMask))
            {

                if (hitleft.collider.gameObject.name == "Task1" && hitright.collider.gameObject.name == "Task1")
                    task1 = true;
                else if (hitleft.collider.gameObject.name == "Task2" && hitright.collider.gameObject.name == "Task2")
                {
                    task2 = true;
                    cart.SetActive(true);

                    cargo1.gameObject.SetActive(true);
                    cargo1.transform.position = cargo1Loc.transform.position;
                    cargo1.transform.rotation = cargo1Loc.transform.rotation;

                    cargo2.transform.position = cargo2Loc.transform.position;
                    cargo2.transform.rotation = cargo2Loc.transform.rotation;
                    cargo2.gameObject.SetActive(true);

                    cargo3.transform.position = cargo3Loc.transform.position;
                    cargo3.transform.rotation = cargo3Loc.transform.rotation;
                    cargo3.gameObject.SetActive(true);

                    cargo4.transform.position = cargo4Loc.transform.position;
                    cargo4.transform.rotation = cargo4Loc.transform.rotation;
                    cargo4.gameObject.SetActive(true);

                }

            }
        }

    }

    void Task1()
    {
        TaskMenu.SetActive(false);
        tm.text = "                 Teleport Back to Spaceship: Press X " + System.Environment.NewLine + "Change Task: Press A";
        

        if (Input.GetKeyDown(KeyCode.X) || (OVRInput.GetDown(OVRInput.RawButton.X)))
        {
            task1 = false;
            instr.text = "";
            teleportBack = true;
        }

        if (Input.GetKeyDown(KeyCode.A) || (OVRInput.GetDown(OVRInput.RawButton.A)))
        {
            task1 = false;
            instr.text = "";
        }

        RaycastHit hitleft;
        RaycastHit hitright;

        instr.text = "Point Ray Towards Light to Turn it On / Off";

        if (Physics.Raycast(leftController.transform.position, leftController.transform.forward, out hitleft, Mathf.Infinity, layerMask) && Physics.Raycast(rightController.transform.position, rightController.transform.forward, out hitright, Mathf.Infinity, layerMask))
        {

            string left = hitleft.collider.gameObject.name;
            string right = hitright.collider.gameObject.name;

            if (left.StartsWith("Light") && left.Equals(right))
            {
                if (togglelight)
                {
                    instr.text = "Point Ray Towards Light to Turn it On / Off" + System.Environment.NewLine + "Lights Turned On: " + lightOn + "/10";

                    if (lights.Contains(left))
                    {
                        lights.Remove(left);
                        lightOn += 1;
                        
                        hitright.collider.gameObject.GetComponent<Light>().enabled = true;
                        LeftRender.SetPosition(0, leftController.transform.position);
                        LeftRender.SetPosition(1, hitleft.collider.transform.position);
                        RightRender.SetPosition(0, rightController.transform.position);
                        RightRender.SetPosition(1, hitleft.collider.transform.position);

                        Debug.DrawLine(leftController.transform.position, hitleft.collider.transform.position);
                        Debug.DrawLine(rightController.transform.position, hitright.collider.transform.position);
                    }
                }
                else if (!lights.Contains(left))
                {
                    lights.Add(left);
                    instr.text = "Point Ray Towards Light to Turn it On / Off" +  System.Environment.NewLine + "Lights Turned Off: " + lights.Count + "/10";
                    //lightOn -= 1;

                    hitright.collider.gameObject.GetComponent<Light>().enabled = false;
                    LeftRender.SetPosition(0, leftController.transform.position);
                    LeftRender.SetPosition(1, hitleft.collider.transform.position);
                    RightRender.SetPosition(0, rightController.transform.position);
                    RightRender.SetPosition(1, hitleft.collider.transform.position);

                    Debug.DrawLine(leftController.transform.position, hitleft.collider.transform.position);
                    Debug.DrawLine(rightController.transform.position, hitright.collider.transform.position);
                }

                if (lights.Count == 10)
                    togglelight = true;

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
        
    }

    void Task2()
    {
        try
        {
            Debug.Log("task2");

            TaskMenu.SetActive(false);
            tm.text = "                 Teleport Back to Spaceship: Press X " + System.Environment.NewLine + "Change Task: Press A";

            if (Input.GetKeyDown(KeyCode.X) || (OVRInput.GetDown(OVRInput.RawButton.X)))
            {
                task2 = false;
                cart.SetActive(false);
                instr.text = "";

                cargo1.gameObject.SetActive(false);
                cargo2.gameObject.SetActive(false);
                cargo3.gameObject.SetActive(false);
                cargo4.gameObject.SetActive(false);

                //Tasks();
                teleportBack = true;
            }

            if (Input.GetKeyDown(KeyCode.A) || (OVRInput.GetDown(OVRInput.RawButton.A)))
            {
                task2 = false;
                cart.SetActive(false);
                instr.text = "";

                cargo1.gameObject.SetActive(false);
                cargo2.gameObject.SetActive(false);
                cargo3.gameObject.SetActive(false);
                cargo4.gameObject.SetActive(false);

                //Tasks();
            }

            RaycastHit hitleft;
            RaycastHit hitright;

            if (selectedObject == null)
            {
                if (Physics.Raycast(leftController.transform.position, leftController.transform.forward, out hitleft, Mathf.Infinity, layerMask) && Physics.Raycast(rightController.transform.position, rightController.transform.forward, out hitright, Mathf.Infinity, layerMask))
                {
                    string left = hitleft.collider.transform.parent.gameObject.name;
                    string right = hitright.collider.transform.parent.gameObject.name;

                    if (left.StartsWith("cargo") && left.Equals(right))
                    {
                        instr.text = "Cargo Selected";
                        cargoLoaded += 1;
                        selectedObject = hitleft.collider.transform.parent.gameObject;
                        selectedObject.transform.SetParent(Planet.transform);
                        objectselected = true;
                        reldist = Vector3.Distance(selectedObject.transform.position, leftController.transform.position);
                    }
                }
            }
            else if (selectedObject != null)
            {
                LeftRender.SetPosition(0, leftController.transform.position);
                LeftRender.SetPosition(1, selectedObject.transform.position);
                
                switch (selectedObject.transform.name)
                {
                    case "cargo1":
                        if (Vector3.Distance(selectedObject.transform.position, cart.transform.FindChild("cargo1cartLoc").position) < 30)
                        {
                            selectedObject.transform.position = cart.transform.Find("cargo1cartLoc").position;
                            instr.text = "Cargo Loaded: " + cargoLoaded + "/4";
                            selectedObject = null;
                            reldist = 0;
                        }
                        break;
                    case "cargo2":
                        if (Vector3.Distance(selectedObject.transform.position, cart.transform.FindChild("cargo2cartLoc").position) < 15)
                        {
                            selectedObject.transform.position = cart.transform.Find("cargo2cartLoc").position;
                            instr.text = "Cargo Loaded: " + cargoLoaded + "/4";
                            selectedObject = null;
                            reldist = 0;
                        }
                        break;
                    case "cargo3":
                        if (Vector3.Distance(selectedObject.transform.position, cart.transform.FindChild("cargo3cartLoc").position) < 15)
                        {
                            selectedObject.transform.position = cart.transform.Find("cargo3cartLoc").position;
                            instr.text = "Cargo Loaded: " + cargoLoaded + "/4";
                            selectedObject = null;
                            reldist = 0;
                        }
                        break;
                    case "cargo4":
                        if (Vector3.Distance(selectedObject.transform.position, cart.transform.Find("cargo4cartLoc").position) < 15)
                        {
                            selectedObject.transform.position = cart.transform.Find("cargo4cartLoc").position;
                            instr.text = "Cargo Loaded: " + cargoLoaded + "/4";
                            selectedObject = null;
                            reldist = 0;
                        }
                        break;
                }
            }
        }
        catch(Exception e)
        {
            tm.text = "Cargo Alignment" + e.StackTrace;
        }
    }
}
