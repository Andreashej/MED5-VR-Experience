using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoHandler : MonoBehaviour
{
    bool btnOnePressed;
    bool btnTwoPressed;

    //Event handler
    public delegate void NewDataEventHandler();
    public static event NewDataEventHandler BtnOnePress;
    public static event NewDataEventHandler BtnTwoPress;

    // Use this for initialization
    void Start()
    {
        Arduino.NewDataEvent += NewData;
        BtnOnePress += Btn1Test; //Testing the eventhandler
        BtnTwoPress += Btn2Test; //Testing the eventhandler
    }

    void Btn1Test() {
        Debug.Log("BTN 1");
    }

    void Btn2Test() {
        Debug.Log("BTN 2");
    }

    void NewData(Arduino arduino)
    {
        if (!btnOnePressed)
        {
            if (arduino.ButtonOne)
            {
                //Debug.Log("BTN 1");

                if (BtnOnePress != null)   //Check that someone is actually subscribed to the event
                    BtnOnePress(); //Subscribe to button one presses by writing ArduinoHandler.BtnOnePress += ...

                btnOnePressed = true;
            }
        }
        else
        {
            if (!arduino.ButtonOne)
                btnOnePressed = false;
        }

        if (!btnTwoPressed)
        {
            if (arduino.ButtonTwo)
            {
                //Debug.Log("BTN 2");

                if (BtnTwoPress != null)   //Check that someone is actually subscribed to the event
                    BtnTwoPress(); //Subscribe to button two presses by writing ArduinoHandler.BtnTwoPress += ...

                btnTwoPressed = true;
            }

        }
        else
        {
            if (!arduino.ButtonTwo)
                btnTwoPressed = false;
        }
    }
}
