using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSTrigger : MonoBehaviour {

	public LightSphere lsPuzzle;
    public GameObject controllerHand;
    private SteamVR_TrackedController controller;
    private bool inCollider = false;

    void Start()
    {
        //initialising the controller and subscribing to the click event
        controller = controllerHand.GetComponent<SteamVR_TrackedController>();
        controller.TriggerClicked += new ClickedEventHandler(RWbtnPressed);
    }

    void RWbtnPressed (object sender, ClickedEventArgs e) //if we're in a collider and click the button we check for conditions
    {
        Debug.Log(inCollider);

        if (inCollider)
        {
            lsPuzzle.CheckConditionsOnButtonPress();
        }
    }

    //when we enter or exit a collider the variable changes between true/false
    void OnTriggerEnter(Collider collision)
    {
        inCollider = true;
    }

    void OnTriggerExit(Collider collision)
    {
        inCollider = false;
    }
}
