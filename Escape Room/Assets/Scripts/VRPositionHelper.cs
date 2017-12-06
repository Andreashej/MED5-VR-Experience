using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPositionHelper : MonoBehaviour
{

    public Transform tracker;
    public bool isTrackerMode;
    public float calibrationTime;
    public float snapshot;
    float elapsedTime;

    void Awake(){
        elapsedTime = 0;
    }

    void Update()
    {
        if (isTrackerMode && elapsedTime <= calibrationTime)
        {
            transform.position = new Vector3(tracker.position.x, transform.position.y, tracker.position.z);
            elapsedTime += Time.deltaTime;
            snapshot = tracker.eulerAngles.z;
        }
    }
}
