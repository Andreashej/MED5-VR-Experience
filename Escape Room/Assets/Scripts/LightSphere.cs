using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LightSphere : MonoBehaviour
{

    public BezierCurve[] curves;
    public Transform sphere;
    public GameObject[] lights;
    public float wiggleRoom; //So the positions don't have to be extremely precise. wiggleRoom has to be at least 1, due to how float values are stored and compared
    [Range(0, 2)]
    public int currentLight; //It shows which light color we need to check for, also used to activate light visuals. 0-2 for the 3 colors so we can use them as array indexes.
    public float[] conditions; //positions where the location is correct, index will be the currentlight
    public float[] savedRotations; //we will store the rotation of the sphere here when we change colors

    void Start()
    {
        lights[0].SetActive(true);
        lights[1].SetActive(false);
        lights[2].SetActive(false);
    }

    void Update()
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
            Debug.Log(sphere.transform.eulerAngles.y);
            Debug.Log(CheckEveryCondition());
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

        //The current light followes a Bezier curve. If we want we can modify it to a spline later so the transition at the 0 degree mark is smoother
        lights[currentLight].transform.LookAt(curves[currentLight].GetPoint(sphere.transform.eulerAngles.y));
    }

    //Checks the condition for the current light selected
    bool CheckCondition() //This could run into an error when the condition is near (closer than "wiggleRoom") 0 degrees, we should make sure this won't happen
    {
        if (sphere.transform.eulerAngles.y == conditions[currentLight] || sphere.transform.eulerAngles.y >= conditions[currentLight] - wiggleRoom && sphere.transform.eulerAngles.y <= conditions[currentLight] + wiggleRoom)
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

    //Changes the current light to the one passed in the light argument
    void ChangeLight(int light)
    {
        lights[currentLight].SetActive(false); //turns off the current light
        lights[light].SetActive(true); //turns on the next light
        savedRotations[currentLight] = sphere.transform.eulerAngles.y; //saves the current rotation
        currentLight = light; //changes currentLight to the new one
        sphere.transform.rotation = Quaternion.Euler(0, savedRotations[light], 0); //loads the saved rotation for the new light
    }
}
