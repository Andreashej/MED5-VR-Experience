using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight_buttonpress : MonoBehaviour
{

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    private SteamVR_TrackedController controller;
    public Vector2 btnCoords;

    // Use this for initialization
    void Start()
    {
        
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        controller = GetComponent<SteamVR_TrackedController>();
        controller.PadClicked += Controller_BtnPress;
    }

    private void Controller_BtnPress(object sender, ClickedEventArgs e)
    {
        if (device.GetAxis().x != 0 || device.GetAxis().y != 0)
        {
            Debug.Log(device.GetAxis().x + " " + device.GetAxis().y);
            btnCoords = new Vector2(device.GetAxis().x, device.GetAxis().y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);
    }
}
