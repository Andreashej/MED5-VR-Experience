using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LightSphere : MonoBehaviour
{

    public float wiggleRoom; //So the positions don't have to be extremely precise.
    [Range(0, 2)]
    public int currentLight; //It shows which light color we need to check for, also used to activate light visuals. 0-2 for the 3 colors so we can use them as array indexes.
    public float[] conditions; //positions where the location is correct, index will be the currentlight
    public float[] savedRotations; //we will store the rotation of the sphere here when we change colors


    void Update()
    {
		if(Input.GetKeyDown("right")){
			transform.Rotate(0,10,0);
		}

		if(Input.GetKeyDown("left")){
			transform.Rotate(0,-10,0);
		}

        if (Input.GetKeyDown("space"))
        {
			Debug.Log(transform.eulerAngles.y);
            Debug.Log(CheckEveryCondition());
        }

		if(Input.GetKeyDown("r")){
			ChangeLight(0);
		}

		if(Input.GetKeyDown("g")){
			ChangeLight(1);
		}

		if(Input.GetKeyDown("b")){
			ChangeLight(2);
		}
    }

    //c will be replaced with currentLight or all light indexes, so we can check conditions individually or all at the same time
    bool CheckCondition() //This could run into an error when the condition is near (closer than "wiggleRoom") 0 degrees, we should make sure this won't happen
    {
        if (transform.eulerAngles.y == conditions[currentLight] || transform.eulerAngles.y >= conditions[currentLight] - wiggleRoom && transform.eulerAngles.y <= conditions[currentLight] + wiggleRoom)
        {
            return true;
        }
        else return false;
    }

    bool CheckCondition(int colorIndex) //This could run into an error when the condition is near (closer than "wiggleRoom") 0 degrees, we should make sure this won't happen
    {
        if (savedRotations[colorIndex] >= conditions[colorIndex] - wiggleRoom && savedRotations[colorIndex] <= conditions[colorIndex] + wiggleRoom)
        {
            return true;
        }
        else return false;
    }

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

    void ChangeLight(int light)
    {
        savedRotations[currentLight] = transform.eulerAngles.y;
        currentLight = light;
		transform.rotation = Quaternion.Euler(0,savedRotations[light],0);
    }
}
