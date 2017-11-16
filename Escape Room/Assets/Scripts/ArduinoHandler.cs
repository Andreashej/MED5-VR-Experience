using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoHandler : MonoBehaviour
{
    bool btnOnePressed;
    bool btnTwoPressed;

    // Use this for initialization
    void Start()
    {
        Arduino.NewDataEvent += NewData;
    }

    void NewData(Arduino arduino)
    {
        if (!btnOnePressed)
        {
            if (arduino.ButtonOne)
            {
                Debug.Log("BTN 1");
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
                Debug.Log("BTN 2");
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
