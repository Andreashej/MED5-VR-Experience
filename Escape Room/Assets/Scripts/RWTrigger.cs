using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RWTrigger : MonoBehaviour
{

    public GameObject controllerHand;
    private SteamVR_TrackedController controller;
    public RuneWheel rwPuzzle;
    private bool inCollider = false;

    void Start()
    {
        controller = controllerHand.GetComponent<SteamVR_TrackedController>();
        controller.TriggerClicked += new ClickedEventHandler(RWbtnPressed);
    }

    void RWbtnPressed (object sender, ClickedEventArgs e)
    {
        Debug.Log(inCollider);

        if (inCollider)
        {
            rwPuzzle.CheckConditionOnButtonPress();   
        }
    }
 
    void OnTriggerEnter(Collider collision)
    {
        inCollider = true;
    }

    void OnTriggerExit(Collider collision)
    {
        inCollider = false;
    }
}
