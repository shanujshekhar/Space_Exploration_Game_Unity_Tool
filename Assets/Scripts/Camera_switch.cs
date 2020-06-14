using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_switch : MonoBehaviour
{

    public OVRCameraRig OVRCamera1;
    public OVRCameraRig OVRCamera2;

    private Camera Component1;
    private Camera Component2;

   
    // Start is called before the first frame update
    void Start()
    {
        Component1 = OVRCamera1.centerEyeAnchor.GetComponent<Camera>();
        Component2 = OVRCamera2.centerEyeAnchor.GetComponent<Camera>();

        Component1.enabled = true;
        Component2.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switchCamera();
    }

    void switchCamera()
    {
        //if (Input.GetKeyDown(KeyCode.C))
        if (Input.GetKeyDown(KeyCode.C) || OVRInput.GetUp(OVRInput.RawButton.X))
        {
            Component1.enabled = !Component1.enabled;
            Component2.enabled = !Component2.enabled;
            
        }
    }
}
