using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LightSphere : MonoBehaviour
{
    public AudioClip btnPressSound;
    public AudioClip unlockChestSound;
    public AudioSource soundSource;
    public GameObject flashlight;
    public Chest chest;
    public GameObject winLight;
    private SteamVR_TrackedController controller;
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    public Vector2 btnCoords;
    public Transform tracker, sphere;
    public GameObject[] lights;
    public float wiggleRoom; //So the positions don't have to be extremely precise. wiggleRoom has to be at least 1, due to how float values are stored and compared
    [Range(0, 2)]
    public int currentLight; //It shows which light color we need to check for, also used to activate light visuals. 0-2 for the 3 colors so we can use them as array indexes.
    public float[] conditions; //positions where the location is correct, index will be the currentlight
    public float[] savedRotations; //we will store the rotation of the sphere here when we change colors

    public float rotationSnaphot;
    public bool isTrackerVersion;
    bool isLit, chestOpen;
    public bool isDone;
    float elapsedTime;
    public float calibrationTime;



    void Start()
    {
        controller = flashlight.GetComponent<SteamVR_TrackedController>();
        trackedObj = flashlight.GetComponent<SteamVR_TrackedObject>();
        controller.PadClicked += Controller_BtnPress;
        ArduinoHandler.BtnOnePress += CheckConditionsOnButtonPress;
        elapsedTime = 0;
    }

    private void Controller_BtnPress(object sender, ClickedEventArgs e)
    {
        if (device.GetAxis().x != 0 || device.GetAxis().y != 0)
        {
            Debug.Log(device.GetAxis().x + " " + device.GetAxis().y);

            btnCoords = new Vector2(device.GetAxis().x, device.GetAxis().y);

            if (btnCoords.x < 0 && btnCoords.y < 0)
            {
                ChangeLight(0);
            }
            else if (btnCoords.x < 0 && btnCoords.y > 0)
            {
                ChangeLight(1);
            }
            else if (btnCoords.x > 0 && btnCoords.y > 0)
            {
                ChangeLight(2);
            }
        }
    }

    void Update()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);

        if (!isDone)
        {
            if (isTrackerVersion && elapsedTime <= calibrationTime) //calibrating the initial rotation of the sphere
            {
                transform.position = new Vector3(tracker.position.x, transform.position.y, tracker.position.z);
                elapsedTime += Time.deltaTime;
                rotationSnaphot = tracker.eulerAngles.z;
            }

            /* //keyboard controls for debugging
            if (Input.GetKeyDown("right"))
            {
                sphere.transform.Rotate(0, 10, 0);
            }

            if (Input.GetKeyDown("left"))
            {
                sphere.transform.Rotate(0, -10, 0);
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

            if (Input.GetKeyDown("space"))
            {
                CheckConditionsOnButtonPress();
                Debug.Log(sphere.transform.eulerAngles.y);
                Debug.Log(isDone);
            } */
        }
        else if (!chestOpen) //if the game is done and the chest is not open
        {
            for (int i = 0; i < 3; i++) //turn on all the lights
            {
                lights[i].SetActive(true);
            }
            winLight.SetActive(true); //shine a light on the chest
            chest.UnlockChest(); //unlock the chest
            chestOpen = true;
            rotationSnaphot = tracker.eulerAngles.z; //take a final rotation snapshot of the tracker
            sphere.localRotation = Quaternion.Euler(0, rotationSnaphot, 0); //set the sphere rotation to the rotation
        }

        //If it's the non-tracker version
        if (!isTrackerVersion)
        {
            lights[currentLight].transform.localRotation = Quaternion.Euler(0, -conditions[currentLight] + sphere.eulerAngles.y, 0);
        }

        //this code is responsible for the rotations when we test the game in "tracker mode" without controllers
        if (isTrackerVersion && !chestOpen)
        {
            sphere.localRotation = Quaternion.Euler(0, tracker.eulerAngles.z - rotationSnaphot + savedRotations[currentLight], 0);
            lights[currentLight].transform.localRotation = Quaternion.Euler(0, sphere.eulerAngles.y - conditions[currentLight], 0);
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
                if (CheckCondition() == false) return false; //checking the condition for the current light
            }
            else if (CheckCondition(i) == false) //checking the conditions for the other lights
            {
                return false;
            }
        }
        return true;
    }

    //this runs when we press the buttons on the pedestal
    public void CheckConditionsOnButtonPress()
    {
        soundSource.PlayOneShot(btnPressSound, 1.0f);
        if (CheckEveryCondition())
        {
            soundSource.PlayOneShot(unlockChestSound, 1.0f);
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
        if (isDone) return;
        isLit = false;
        rotationSnaphot = tracker.eulerAngles.z; //Takes a snapshot of the tracker's current rotation so we can switch the projections' orientations
        savedRotations[currentLight] = sphere.eulerAngles.y; //saves the current rotation
        currentLight = light; //changes currentLight to the new one
        if (!isTrackerVersion) sphere.rotation = Quaternion.Euler(0, savedRotations[light], 0); //loads the saved rotation for the new light
    }

    public void TurnLightOn()
    {
        if (isDone) return; //If the game is done it won't run
        if (!isLit) //if the light isn't alredy on
        {
            Debug.Log("light on");
            for (int i = 0; i < 3; i++) lights[i].SetActive(false); //turns off every light
            lights[currentLight].SetActive(true); //turns back on the current one
        }
        isLit = true;
    }

    public void TurnLightOff()
    {
        if (isDone) return; //If the game is done it won't run
        for (int i = 0; i < 3; i++) lights[i].SetActive(false); //turns every light off
        isLit = false;
    }

}
