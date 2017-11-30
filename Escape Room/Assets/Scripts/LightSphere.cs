using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LightSphere : MonoBehaviour
{

    public Transform tracker, sphere;
    public GameObject[] lights;
    public BezierCurve[] curves;
    public float wiggleRoom; //So the positions don't have to be extremely precise. wiggleRoom has to be at least 1, due to how float values are stored and compared
    [Range(0, 2)]
    public int currentLight; //It shows which light color we need to check for, also used to activate light visuals. 0-2 for the 3 colors so we can use them as array indexes.
    public float[] conditions; //positions where the location is correct, index will be the currentlight
    public float[] savedRotations; //we will store the rotation of the sphere here when we change colors

    public float rotationSnaphot;
    public bool isTrackerVersion;
    bool isLit, chestOpen;
    public bool isDone;

    void Start()
    {
        ArduinoHandler.BtnOnePress += CheckConditionsOnButtonPress;
    }

    void Update()
    {
        if (!isDone)
        {
            //keyboard controls
            if (Input.GetKeyDown("right"))
            {
                sphere.transform.Rotate(0, 10, 0);
            }

            if (Input.GetKeyDown("left"))
            {
                sphere.transform.Rotate(0, -10, 0);
            }

            if (Input.GetKeyDown("space"))
            {
                CheckEveryCondition();
                Debug.Log(sphere.transform.eulerAngles.y);
                Debug.Log(isDone);
            }

            if (Input.GetKeyDown("r"))
            {
                ChangeLight(0);
            }

            if (Input.GetKeyDown("g"))
            {
                ChangeLight(1);
            }

            if (Input.GetKeyDown("b"))
            {
                ChangeLight(2);
            }
            else if(!chestOpen)
            {
                //open chest
                chestOpen = true;
            }
        }

        //The current light followes a Bezier curve. If we want we can modify it to a spline later so the transition at the 0 degree mark is smoother
        lights[currentLight].transform.LookAt(curves[currentLight].GetPoint(sphere.transform.eulerAngles.y));

        //this code is responsible for the rotations when we test the game in "tracker mode" without controllers
        if (isTrackerVersion)
        {
            sphere.rotation = Quaternion.Euler(0, tracker.eulerAngles.z - rotationSnaphot + savedRotations[currentLight], 0);
        }
    }

    //Checks the condition for the current light selected
    bool CheckCondition() //This could run into an error when the condition is near (closer than "wiggleRoom") 0 degrees, we should make sure this won't happen
    {
        if (sphere.eulerAngles.y == conditions[currentLight] || sphere.eulerAngles.y >= conditions[currentLight] - wiggleRoom && sphere.eulerAngles.y <= conditions[currentLight] + wiggleRoom)
        {
            return true;
        }
        else return false;
    }

    //Checks the condition for any given light passed through colorIndex.
    bool CheckCondition(int colorIndex) //This could run into an error when the condition is near (closer than "wiggleRoom") 0 degrees, we should make sure this won't happen
    {
        if (savedRotations[colorIndex] >= conditions[colorIndex] - wiggleRoom && savedRotations[colorIndex] <= conditions[colorIndex] + wiggleRoom)
        {
            return true;
        }
        else return false;
    }

    //Checks the current condition and the remaining two as well.
    bool CheckEveryCondition()
    {
        for (int i = 0; i < conditions.Length; i++)
        {
            if (i == currentLight)
            {
                if (CheckCondition() == false) return false;
            }
            else if (CheckCondition(i) == false)
            {
                return false;
            }
        }
        return true;
    }

    void CheckConditionsOnButtonPress()
    {
        if (CheckEveryCondition())
        {
            isDone = true;
            Debug.Log("done");
            return;
        }
        else
        {
            Debug.Log("not done");
            return;
        }
    }

    //Changes the current light to the one passed in the light argument
    void ChangeLight(int light)
    {
        isLit = false;
        rotationSnaphot = tracker.eulerAngles.z;
        //lights[currentLight].SetActive(false); //turns off the current light
        //lights[light].SetActive(true); //turns on the next light
        savedRotations[currentLight] = sphere.eulerAngles.y; //saves the current rotation
        currentLight = light; //changes currentLight to the new one
        sphere.rotation = Quaternion.Euler(0, savedRotations[light], 0); //loads the saved rotation for the new light
    }

    public void TurnLightOn()
    {
        if (!isLit)
        {
            Debug.Log("light on");
            for (int i = 0; i < 3; i++) lights[i].SetActive(false);
            lights[currentLight].SetActive(true);
        }
        isLit = true;
    }

    public void TurnLightOff()
    {
        for (int i = 0; i < 3; i++) lights[i].SetActive(false);
        isLit = false;
    }

}
