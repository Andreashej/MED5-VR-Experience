using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneWheel : MonoBehaviour
{
    public AudioClip lightBulbSound;
    public AudioClip unlockDoorSound;
    public AudioSource soundSource;
    public Transform tracker;
    public Transform[] rings;
    public Material[] materials;
    public GameObject leftButton;
    public Door door;
    int middleDisplacement, outerDisplacement;
    float increments = 360f / 26f;
    public PassCode[] passcode;
    public bool middleRingActive = true;
    bool puzzleDone = false;
    int progress = 0;
    public float rotationSnapshot;

    public bool isTrackerVersion = false;
    public float[] savedRotations;

    public GameObject[] lights;

    void Start()
    {
        //intitialising the saved rotations
        savedRotations[0] = 0f;
        savedRotations[1] = 0f;
        //subscribing to the arduino button presses
        ArduinoHandler.BtnOnePress += CheckConditionOnButtonPress;
        ArduinoHandler.BtnTwoPress += ChangeWheel;

    }


    void Update()
    {
        if (progress < passcode.Length) //if it's not done yet
        {
            //Displacement calculation
            //This still calculates the displacement of the middle ring, even though we disabled it in the test.
            middleDisplacement = Mathf.FloorToInt(26 * (rings[1].eulerAngles.y + increments / 2f) / 360f);
            if (middleDisplacement == 26) middleDisplacement = 0; //Because of the calculation, -6.92 to 0 degrees is 26 instead of 0, this is to correct that.
            outerDisplacement = Mathf.FloorToInt(26 * (rings[0].eulerAngles.y + increments / 2f) / 360f);
            if (outerDisplacement == 26) outerDisplacement = 0; //Same if statement as with the middle ring

            //This runs if the tracker version is being played.
            if (isTrackerVersion)
            {
                rings[BoolToInt(middleRingActive)].rotation = Quaternion.Euler(0, tracker.eulerAngles.z - rotationSnapshot + savedRotations[BoolToInt(middleRingActive)], 0);
            }

            /* //Keyboard inputs for debugging purposes

            if (Input.GetKeyDown("right")) //moving the disk right
            {
                if (middleRingActive) rings[1].transform.Rotate(0, increments, 0);
                else rings[0].transform.Rotate(0, increments, 0);
            }
            if (Input.GetKeyDown("left")) //moving the disc left
            {
                if (middleRingActive) rings[1].transform.Rotate(0, -increments, 0);
                else rings[0].transform.Rotate(0, -increments, 0);
            }

            //Passcode input, if the two wheels are in the correct posiotion the progress counter goes up
            if (Input.GetKeyDown("space"))
            {
                Debug.Log(middleDisplacement);
                Debug.Log(outerDisplacement);
                if (CheckCondition(progress))
                {
                    //Add visual representation if it's gewd
                    Debug.Log("right");
                    progress++;
                    lights[progress - 1].SetActive(true);
                    soundSource.PlayOneShot(lightBulbSound, 1.0f);
                }
                else
                {
                    Debug.Log("wrong");
                    //add visual representation if not good
                }
            }
            //Wheelchange input
            if (Input.GetKeyDown("return")) ChangeWheel(); //This will be a button to change between the two rings */
        }
        else if (!puzzleDone) //if we did the puzzle
        {
            soundSource.PlayOneShot(unlockDoorSound, 1.0f);
            Debug.Log("DONE"); //this will be the door opening thing
            door.ActivateDoor();
            puzzleDone = true; //we only have to do it once
            ArduinoHandler.BtnOnePress -= CheckConditionOnButtonPress;
        }
    }



    bool CheckCondition(int conditionIndex) //checks the runewheel's current positions to the passcode positions of the given index
    {
        return (middleDisplacement == passcode[conditionIndex].middleSymbol && outerDisplacement == passcode[conditionIndex].outerSymbol);
    }

    //This method runs when you click the button on the pedestal
    public void CheckConditionOnButtonPress()
    {
        if (CheckCondition(progress))
        {
            //This runs when the position of the ring is correct
            Debug.Log("right");
            progress++;
            lights[progress - 1].SetActive(true);
            soundSource.PlayOneShot(lightBulbSound, 1.0f);
        }
        else
        {
            Debug.Log("wrong");
            //nothing happens if the input is wrong
        }
    }

    //In the tracker version a second button can be used to change which ring we want to rotate. The method is obsolete because we don't use two rings in the test.
    void ChangeWheel()
    {
        rotationSnapshot = tracker.eulerAngles.z;
        savedRotations[BoolToInt(middleRingActive)] = rings[BoolToInt(middleRingActive)].transform.eulerAngles.y;
        leftButton.GetComponent<Renderer>().material = materials[BoolToInt(middleRingActive)];
        middleRingActive = !middleRingActive;
    }

    //Simple boolean to integer conversion, to convert middleRingActive to an array index.
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