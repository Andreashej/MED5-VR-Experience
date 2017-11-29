using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject lid;
    public Transform tracker;
    public bool isTrackerVersion, isLocked;

    void Start()
    {
		isLocked = true;
        lid.GetComponent<Valve.VR.InteractionSystem.CircularDrive>().enabled = false;
    }

    void Update()
    {
        if (isTrackerVersion)
        {
            if (!isLocked)
            {
                //I will put the rotation of the racker here
            }
        }
    }

    public void UnlockChest()
    {
        isLocked = false;
        lid.GetComponent<Valve.VR.InteractionSystem.CircularDrive>().enabled = true;
    }
}
