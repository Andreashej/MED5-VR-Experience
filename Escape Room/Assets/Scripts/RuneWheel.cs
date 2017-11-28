using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneWheel : MonoBehaviour
{
    public Transform tracker;
    public Transform[] rings;
    int middleDisplacement, outerDisplacement;
    float increments = 360f / 26f;
    public PassCode[] passcode;
    bool middleRingActive = true;
    bool puzzleDone = false;
    int progress = 0;
    public float rotationSnapshot;

    public bool isTrackerVersion = false;
    public float[] savedRotations;

    void Start()
    {
        savedRotations[0] = 0f;
        savedRotations[1] = 0f;
    }


    void Update()
    {
        if (progress < passcode.Length) //if it's not done yet
        {
            //Passcode input, if the two wheels are in the correct posiotion the progress counter goes up
            if (Input.GetKeyDown("space")) //test input, will be a button
            {
                if (checkCondition(progress))
                {
                    //Add visual representation if it's gewd
                    Debug.Log("right");
                    progress++;
                }
                else
                {
                    Debug.Log("wrong");
                    //add visual representation if not good
                }
            }
            if (Input.GetKeyDown("return")) ChangeWheel(); //This will be a button to change between the two rings


            middleDisplacement = Mathf.FloorToInt(26 * (rings[1].eulerAngles.y + increments / 2f) / 360f);
            if (middleDisplacement == 26) middleDisplacement = 0; //Because of the calculation, -6.92 to 0 degrees is 26 instead of 0, this is to correct that.
            outerDisplacement = Mathf.FloorToInt(26 * (rings[0].eulerAngles.y + increments / 2f) / 360f);
            if (outerDisplacement == 26) outerDisplacement = 0; //Same if statement as with the middle ring


            if (isTrackerVersion)
            {
                transform.position = tracker.position;
                rings[BoolToInt(middleRingActive)].rotation = Quaternion.Euler(0, tracker.eulerAngles.z - rotationSnapshot + savedRotations[BoolToInt(middleRingActive)], 0);
            }

            //Inputs for testing the puzzle, it will be replaced with VR moving things.
            /*
            if (Input.GetKeyDown("right")) //moving the disk right
            {
                if (middleRingActive) rings[1].transform.Rotate(0, increments, 0);
                else rings[0].transform.Rotate(0, increments, 0);
            }
            if (Input.GetKeyDown("left")) //moving the disc left
            {
                if (middleRingActive) rings[1].transform.Rotate(0, -increments, 0);
                else rings[0].transform.Rotate(0, -increments, 0);
            }*/

            //Debug.Log(middleRing.eulerAngles.y);
            //Debug.Log(middleDisplacement);



        }
        else if (!puzzleDone) //if we did the puzzle
        {
            Debug.Log("DONE"); //this will be the door opening thing
            puzzleDone = true; //we only have to do it once
        }
    }

    bool checkCondition(int conditionIndex) //checks the runewheel's current positions to the passcode positions of the given index
    {
        return (middleDisplacement == passcode[conditionIndex].middleSymbol && outerDisplacement == passcode[conditionIndex].outerSymbol);
    }

    void ChangeWheel()
    {
        rotationSnapshot = tracker.eulerAngles.z;
        savedRotations[BoolToInt(middleRingActive)] = rings[BoolToInt(middleRingActive)].transform.eulerAngles.y;
        middleRingActive = !middleRingActive;
    }

    int BoolToInt(bool b)
    {
        if (b) return 1;
        else return 0;
    }

}

[System.Serializable]
public struct PassCode //struct for passcode checking, essentially the same as the one for tile checking but with different labels for ease of reading
{
    public int middleSymbol;
    public int outerSymbol;
}