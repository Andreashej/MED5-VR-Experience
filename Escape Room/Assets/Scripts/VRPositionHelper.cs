using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPositionHelper : MonoBehaviour
{

    public Transform tracker;
    public bool isTrackerMode;
    public float calibrationTime;
    float elapsedTime;

    void Awake(){
        elapsedTime = 0;
    }

    void Update()
    {
        //Here we calculate the initial position of the object attached to the tracker. it's set for 2 seconds in the project because on awake the position is not correct immediately
        if (isTrackerMode && elapsedTime <= calibrationTime)
        {
            transform.position = new Vector3(tracker.position.x, transform.position.y, tracker.position.z);
            elapsedTime += Time.deltaTime;
        }
    }
}
