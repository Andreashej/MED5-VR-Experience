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
        controller = controllerHand.GetComponent<SteamVR_TrackedController>(); //initialising the controller and subscribing to the click event
        controller.TriggerClicked += new ClickedEventHandler(RWbtnPressed);
    }

    void RWbtnPressed (object sender, ClickedEventArgs e) //if we're in a collider and click the button then it checks for conditions
    {
        Debug.Log(inCollider);

        if (inCollider)
        {
            rwPuzzle.CheckConditionOnButtonPress();   
        }
    }
 
    void OnTriggerEnter(Collider collision)
    {
        inCollider = true; //when we enter a collider it becomes true
    }

    void OnTriggerExit(Collider collision)
    {
        inCollider = false; //when we exit a collider we change it to false
    }
}
