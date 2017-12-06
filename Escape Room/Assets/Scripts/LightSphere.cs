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

            if (btnCoords.x < 0 && btnCoords.y > 0)
            {
                ChangeLight(1);
            }
            else if (btnCoords.x < 0 && btnCoords.y < 0)
            {
                ChangeLight(0);
            }
            else if (btnCoords.x > 0 && btnCoords.y > 0)
            {
                ChangeLight(2);
            }
            else if (btnCoords.x > 0 && btnCoords.y < 0)
            {

            }
        }
    }

    void Update()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);
        //if (lightReveal != null) Debug.Log(lightReveal.activeLightID);
        if (!isDone)

        {

            if (isTrackerVersion && elapsedTime <= calibrationTime)
        {
            transform.position = new Vector3(tracker.position.x, transform.position.y, tracker.position.z);
            elapsedTime += Time.deltaTime;
            rotationSnaphot = tracker.eulerAngles.z;
        }

            //keyboard controls
            /*if (Input.GetKeyDown("right"))
            {
                sphere.transform.Rotate(0, 10, 0);
            }

            if (Input.GetKeyDown("left"))
            {
                sphere.transform.Rotate(0, -10, 0);
            }
*/
            if (Input.GetKeyDown("space"))
            {
                CheckConditionsOnButtonPress();
                Debug.Log(sphere.transform.eulerAngles.y);
                Debug.Log(isDone);
            }

            /*if (Input.GetKeyDown("r"))
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
            }*/

        }
        else if (!chestOpen)
        {
            for (int i = 0; i < 3; i++)
            {
                lights[i].SetActive(true);
            }
            winLight.SetActive(true);
            chest.UnlockChest();
            chestOpen = true;
            rotationSnaphot = tracker.eulerAngles.z;
            sphere.localRotation = Quaternion.Euler(0,rotationSnaphot,0);
        }

        if (currentLight == -1) TurnLightOff();
        //The current light followes a Bezier curve. If we want we can modify it to a spline later so the transition at the 0 degree mark is smoother

        if (!isTrackerVersion && currentLight != -1) lights[currentLight].transform.localRotation = Quaternion.Euler(0, -conditions[currentLight] + sphere.eulerAngles.y, 0);

        //this code is responsible for the rotations when we test the game in "tracker mode" without controllers
        if (isTrackerVersion && currentLight != -1 && !chestOpen)
        {
            sphere.localRotation =  Quaternion.Euler(0, tracker.eulerAngles.z - rotationSnaphot + savedRotations[currentLight], 0);
            lights[currentLight].transform.localRotation = Quaternion.Euler(0, sphere.eulerAngles.y - conditions[currentLight],0);
            //lights[currentLight].transform.localRotation = Quaternion.Euler(0, tracker.eulerAngles.z - rotationSnaphot + savedRotations[currentLight], 0);
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
        if(isDone) return;
        isLit = false;
        rotationSnaphot = tracker.eulerAngles.z;
        //lights[currentLight].SetActive(false); //turns off the current light
        //lights[light].SetActive(true); //turns on the next light
        savedRotations[currentLight] = sphere.eulerAngles.y; //saves the current rotation
        currentLight = light; //changes currentLight to the new one
        if (!isTrackerVersion) sphere.rotation = Quaternion.Euler(0, savedRotations[light], 0); //loads the saved rotation for the new light
    }

    public void TurnLightOn()
    {
        if(isDone) return;
        if (currentLight == -1) return;
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
        if(isDone) return;
        for (int i = 0; i < 3; i++) lights[i].SetActive(false);
        isLit = false;
    }

}
