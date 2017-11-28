using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPositionHelper : MonoBehaviour
{

    public Transform tracker;
    public bool isTrackerMode;

    void Update()
    {
        if (isTrackerMode)
        {
            transform.position = tracker.position;
			transform.position = new Vector3(transform.position.x, transform.position.y-0.8f, transform.position.z);
        }
    }
}
